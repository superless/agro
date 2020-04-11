using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.enums.input;
using trifenix.agro.enums.searchModel;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.util;
using trifenix.agro.validator.interfaces;

namespace trifenix.agro.external.operations.entities.ext {

    public class ProductOperations : MainOperation<Product, ProductInput>, IGenericOperation<Product, ProductInput> {

        private readonly IGenericOperation<Dose, DosesInput> dosesOperation;
        private readonly ICommonQueries queries;

        public ProductOperations(IMainGenericDb<Product> repo, IExistElement existElement, IAgroSearch search, IGenericOperation<Dose, DosesInput> dosesOperation, ICommonDbOperations<Product> commonDb, ICommonQueries queries, IValidator validators) : base(repo, existElement, search, commonDb, validators) {
            this.dosesOperation = dosesOperation;
            this.queries = queries;
        }
        
        public async Task Remove(string id) { }

        //private RelatedId[] GetIdsRelated(Product product) {
        //    var list = new List<RelatedId> {
        //        new RelatedId { EntityIndex = (int)EntityRelated.INGREDIENT, EntityId = product.IdActiveIngredient }
        //    };
        //    return list.ToArray();
        //}

        //private Property[] GetElementRelated(Product product) {
        //    var list = new List<Property>();
        //    if (!string.IsNullOrWhiteSpace(product.Brand))
        //        list.Add(new Property { PropertyIndex = (int)PropertyRelated.GENERIC_BRAND, Value = product.Brand });
        //    list.Add(new Property { PropertyIndex = (int)PropertyRelated.GENERIC_NAME, Value = product.Name });
        //    return list.ToArray();
        //}

        private async Task<string> CreateDefaultDoses(string idProduct) {
            var dosesInput = new DosesInput {
                IdProduct = idProduct,
                Active = true,
                Default = true
            };
            var result = await dosesOperation.SaveInput(dosesInput, false);
            return result.IdRelated;
        }
        
        private async Task<RelatedId> RemoveDoses(Product product) {
            if (!string.IsNullOrWhiteSpace(product.Id)) {
                //obtiene el identificador de la dosis por defecto
                var defaultDoses = await queries.GetDefaultDosesId(product.Id);

                if (string.IsNullOrWhiteSpace(defaultDoses))
                    defaultDoses = await CreateDefaultDoses(product.Id);

                // elimina todas las dosis que no sean por defecto relacionadas con el producto
                search.DeleteElementsWithRelatedElementExceptId(EntityRelated.DOSES, EntityRelated.PRODUCT, product.Id, defaultDoses);

                // obtiene todas las dosis que no sean por defecto
                var dosesPrevIds = await queries.GetActiveDosesIdsFromProductId(product.Id);
                if (dosesPrevIds.Any())
                    foreach (var idDoses in dosesPrevIds)
                        // elimina cada dosis, internamente elimina si no hay dependencias, si existen dependencias la desactiva y la deja en el search.
                        await dosesOperation.Remove(idDoses);

                return new RelatedId { EntityIndex = (int)EntityRelated.DOSES, EntityId = defaultDoses };
            }
            else {
                var dosesDefaultId = await CreateDefaultDoses(product.Id);
                return new RelatedId {
                    EntityIndex = (int)EntityRelated.DOSES,
                    EntityId = dosesDefaultId
                };
            }
        }

        public async Task<ExtPostContainer<string>> SaveInput(ProductInput productInput, bool isBatch) {
            await Validate(productInput);
            var id = !string.IsNullOrWhiteSpace(productInput.Id) ? productInput.Id : Guid.NewGuid().ToString("N");
            var product = new Product {
                Id = id,
                Brand = productInput.Brand,
                Name = productInput.Name,
                IdActiveIngredient = productInput.IdActiveIngredient,
                KindOfBottle = productInput.KindOfBottle,
                MeasureType = productInput.MeasureType,
                Quantity = productInput.Quantity
            };
            var doses = productInput.Doses.Select(dose => {
                dose.IdProduct = id;
                return dose;
            });
            if (!isBatch) {
                await Save(product);
                foreach (var dose in doses)
                    await dosesOperation.SaveInput(dose, false);
            } else {
                await repo.CreateEntityContainer(product);
                foreach (var dose in doses)
                    await dosesOperation.SaveInput(dose, true);
            }
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> Save(Product product) {
            await repo.CreateUpdate(product);
          
            var dosesDefault = await RemoveDoses(product);
            var productSearch = search.GetEntitySearch(product).LastOrDefault();
            var dosesFound = search.GetElementsWithRelatedElement(EntityRelated.DOSES, EntityRelated.PRODUCT, product.Id);

            var doses = new List<RelatedId> {
                dosesDefault
            };

            if (dosesFound.Any()) doses.AddRange(dosesFound.Select(d => new RelatedId { EntityId = d.Id, EntityIndex = (int)EntityRelated.DOSES }));

            productSearch.RelatedIds = productSearch.RelatedIds.Add(doses);
          
            search.AddElements(new List<EntitySearch> { productSearch });
               
            return new ExtPostContainer<string> {
                IdRelated = product.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}