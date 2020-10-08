using System;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm.search.model;

namespace trifenix.connect.agro.external
{

    public class ProductOperations<T> : MainOperation<Product, ProductInput, T>, IGenericOperation<Product, ProductInput> {

        private readonly IGenericOperation<Dose, DosesInput> dosesOperation;
        private readonly ICommonQueries queries;

        public ProductOperations(IMainGenericDb<Product> repo, IAgroSearch<T> search, IGenericOperation<Dose, DosesInput> dosesOperation, ICommonDbOperations<Product> commonDb, ICommonQueries queries, IValidatorAttributes<ProductInput, Product> validator) : base(repo, search, commonDb, validator) {
            this.dosesOperation = dosesOperation;
            this.queries = queries;
        }
        
        public async Task Remove(string id) {
            await repo.DeleteEntity(id);
        }

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
                    foreach (var idDoses in dosesPrevIds) { 
                        // elimina cada dosis, internamente elimina si no hay dependencias, si existen dependencias la desactiva y la deja en el search.
                        await dosesOperation.Remove(idDoses);
                    }
                return new RelatedId { index = (int)EntityRelated.DOSES, id = defaultDoses };
            }
            else {
                var dosesDefaultId = await CreateDefaultDoses(product.Id);
                return new RelatedId {
                    index = (int)EntityRelated.DOSES,
                    id = dosesDefaultId
                };
            }
        }

        public async Task<ExtPostContainer<string>> SaveInput(ProductInput productInput, bool isBatch) {

            await Validate(productInput);


            var id = !string.IsNullOrWhiteSpace(productInput.Id) ? productInput.Id : Guid.NewGuid().ToString("N");


            var product = new Product {
                Id = id,
                IdBrand = productInput.IdBrand,
                Name = productInput.Name,
                IdActiveIngredient = productInput.IdActiveIngredient,
                SagCode = productInput.SagCode,
                MeasureType = productInput.MeasureType,
                
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
            search.AddDocument(product);
            await RemoveDoses(product);
            return new ExtPostContainer<string> {
                IdRelated = product.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}