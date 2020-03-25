using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.helper;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.ext
{
    public class ProductOperations : MainReadOperationName<Product, ProductInput>, IGenericOperation<Product, ProductInput>
    {
        private readonly IGenericOperation<Doses, DosesInput> dosesOperation;
        private readonly ICommonQueries queries;

        public ProductOperations(IMainGenericDb<Product> repo, IExistElement existElement, IAgroSearch search, IGenericOperation<Doses, DosesInput> dosesOperation, ICommonDbOperations<Product> commonDb, ICommonQueries queries) : base(repo, existElement, search, commonDb)
        {
            this.dosesOperation = dosesOperation;
            this.queries = queries;
        }

        public async Task Remove(string id)
        {
            
        }

        private RelatedId[] GetIdsRelated(ProductInput input) {

            var list = new List<RelatedId>();

            if (!string.IsNullOrWhiteSpace(input.IdActiveIngredient))
            {
                list.Add(new RelatedId { EntityIndex = (int)EntityRelated.INGREDIENT, EntityId = input.IdActiveIngredient });
            }
            return list.ToArray();
        }

        private Property[] GetElementRelated(ProductInput input) {
            var list = new List<Property>();

            if (!string.IsNullOrWhiteSpace(input.Brand)) {
                list.Add(new Property { PropertyIndex = (int)PropertyRelated.GENERIC_BRAND, Value = input.Brand });
            }
                
            list.Add(new Property { PropertyIndex = (int)PropertyRelated.GENERIC_NAME, Value = input.Name });

            return list.ToArray();

        }

        private async Task<string> ValidaProduct(ProductInput productInput) {
            if (productInput.Doses != null && productInput.Doses.Any())
            {
                var stringError = productInput.Doses.Select(s => DosesHelper.ValidaDoses(existElement, s));

                if (stringError.Any(s=>!string.IsNullOrWhiteSpace(s)))
                {
                    return string.Join(",", stringError.Where(s => !string.IsNullOrWhiteSpace(s)));
                }


            }
            var existsIngredient = await existElement.ExistsById<Ingredient>(productInput.IdActiveIngredient);

            if (!existsIngredient) return "no existe id de ingrediente";

            return string.Empty;
        }


        private EntitySearch CreateSearchDefaultDoses(string idProduct, string idDoses) {
            return new EntitySearch
            {
                Id = idDoses,
                EntityIndex = (int)EntityRelated.DOSES,
                Created = DateTime.Now,
                References = new RelatedId[]{
                        new RelatedId{
                            EntityId = idProduct,
                            EntityIndex = (int)EntityRelated.PRODUCT,
                        }
                    },
                RelatedEnumValues = new RelatedEnumValue[]{
                        new RelatedEnumValue{
                            EnumerationIndex = (int)EnumerationRelated.GENERIC_ACTIVE,
                            Value = 1
                        },
                        new RelatedEnumValue{
                            EnumerationIndex = (int)EnumerationRelated.GENERIC_DEFAULT,
                            Value = 1
                        },
                    }
            };
        }
        private async Task<string> CreateDefaultDoses(string idProduct) {

            var id = Guid.NewGuid().ToString("N");
            await dosesOperation.Store.CreateUpdate(new Doses
            {
                Id = id,
                IdProduct = idProduct,
                LastModified = DateTime.Now,
                Active = true,
                Default = true

            });

            

            search.AddElements(new List<EntitySearch>
            {
                CreateSearchDefaultDoses(idProduct, id)
            });
            return id;

        }


        private async Task<RelatedId> RemoveDoses(ProductInput input, string id) {
            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                //obtiene el identificador de la dosis por defecto
                var defaultDoses = await queries.GetDefaultDosesId(input.Id);

                

                // elimina todas las dosis que no sean por defecto relacionadas con el producto
                search.DeleteElementsWithRelatedElementExceptId(EntityRelated.DOSES, EntityRelated.PRODUCT, id, defaultDoses);

                var defaultDosesLocal = search.GetEntity(EntityRelated.DOSES, defaultDoses);

                if (string.IsNullOrWhiteSpace(defaultDoses))
                {
                    defaultDoses = await CreateDefaultDoses(id);

                } else if (defaultDosesLocal == null)
                {
                    search.AddElements(new List<EntitySearch> {
                        CreateSearchDefaultDoses(input.Id, defaultDoses)
                    });
                }





                // obtiene todas las dosis que no sean por defecto
                var dosesPrevIds = await queries.GetActiveDosesIdsFromProductId(input.Id);

                if (dosesPrevIds.Any())
                {
                    foreach (var idDoses in dosesPrevIds)
                    {
                        // elimina cada dosis, internamente elimina si no hay dependencias, si existen dependencias la desactiva y la deja en el search.
                        await dosesOperation.Remove(idDoses);
                    }
                }

                return new RelatedId { EntityIndex = (int)EntityRelated.DOSES, EntityId = defaultDoses };
            }
            else {
                var dosesDefaultId = await CreateDefaultDoses(id);
                return new RelatedId
                {
                    EntityIndex = (int)EntityRelated.DOSES,
                    EntityId = dosesDefaultId
                };
            }
        }


        public async Task<ExtPostContainer<string>> Save(ProductInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var product = new Product
            {
                Id = id,
                Brand = input.Brand,
                Name = input.Name,
                IdActiveIngredient = input.IdActiveIngredient,
                KindOfBottle = input.KindOfBottle,
                MeasureType = input.MeasureType,
                Quantity = input.Quantity
            };

            // valida
            var valida = await Validate(input);
            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, product.CosmosEntityName));

            //valida producto
            var validaProd = await ValidaProduct(input);
            if (!string.IsNullOrWhiteSpace(validaProd)) throw new Exception(validaProd);

            

            // obtiene el id de ingrediente 
            var relatedIds = GetIdsRelated(input).ToList();

            //Remueve doses y retorna la dosis por defecto
            var dosesDefault = await RemoveDoses(input, id);

            relatedIds.Add(dosesDefault);

            await repo.CreateUpdate(product);

            if (input.Doses != null && input.Doses.Any())
            {
                foreach (var dose in input.Doses)
                {
                    dose.IdProduct = id;
                    dose.Default = false;
                    dose.Active = true;
                    var idDoses = await dosesOperation.Save(dose);
                    relatedIds.Add(new RelatedId
                    {
                        EntityId = idDoses.IdRelated,
                        EntityIndex = (int)EntityRelated.DOSES
                    });
                }

            }

            



            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.PRODUCT,
                    Created = DateTime.Now,
                    RelatedProperties = GetElementRelated(input),
                    References = relatedIds.ToArray(),
                    RelatedEnumValues = new RelatedEnumValue[]{
                        new RelatedEnumValue{ EnumerationIndex = (int)EnumerationRelated.PRODUCT_KINDOFBOTTLE, Value= (int)input.KindOfBottle },
                        new RelatedEnumValue{ EnumerationIndex = (int)EnumerationRelated.PRODUCT_MEASURETYPE, Value= (int)input.MeasureType }
                    }

                }
            });




            return new ExtPostContainer<string>
            {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }
    }

}
