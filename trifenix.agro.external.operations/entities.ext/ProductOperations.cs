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

        public ProductOperations(IMainGenericDb<Product> repo, IExistElement existElement, IAgroSearch search, IGenericOperation<Doses, DosesInput> dosesOperation, ICommonDbOperations<Product> commonDb) : base(repo, existElement, search, commonDb)
        {
            this.dosesOperation = dosesOperation;
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
            if (!string.IsNullOrWhiteSpace(input.Brand))
                list.Add(new Property { PropertyIndex = (int)PropertyRelated.GENERIC_BRAND, Value = input.Brand });
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

        private async Task<string> CreateDefaultDoses(string idProduct) {

            var id = Guid.NewGuid().ToString("N");
            await dosesOperation.Store.CreateUpdate(new Doses
            {
                Id = id,
                IdProduct = idProduct,
                Active = true,
                Default = true

            });

            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.DOSES,
                    Created = DateTime.Now,
                    RelatedIds = new RelatedId[]{ 
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
                }
            });
            return id;

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


            var validaProd = await ValidaProduct(input);
            if (!string.IsNullOrWhiteSpace(validaProd)) throw new Exception(validaProd);

            


            var relatedIds = GetIdsRelated(input).ToList();

            if (!string.IsNullOrWhiteSpace(input.Id))
            {
                var query = $"SELECT DISTINCT value c.id from c where c.IdProduct='{input.Id}' and c.Active=true and c.Default=false";

                var dosesPrevIds = await dosesOperation.Store.Store.QueryMultipleAsync<string>(query);

                if (dosesPrevIds.Any())
                {
                    foreach (var idDoses in dosesPrevIds)
                    {
                        await dosesOperation.Remove(idDoses);
                    }
                }

                query = $"SELECT DISTINCT value c.id from c where c.IdProduct='{input.Id}' and c.Active=true and c.Default=true";

                var defaultDoses = await dosesOperation.Store.Store.QuerySingleAsync<string>(query);

                relatedIds.Add(new RelatedId { EntityIndex = (int)EntityRelated.DOSES, EntityId = defaultDoses });

                var queryDelete = $"EntityIndex eq {(int)EntityRelated.DOSES} and  Id ne '{defaultDoses}' and RelatedIds/any(elementId: elementId/EntityIndex eq {(int)EntityRelated.PRODUCT} and elementId/EntityId eq '{id}')";

                var elements = search.FilterElements<EntitySearch>(queryDelete);
                if (elements.Any())
                {
                    search.DeleteElements(elements);
                }




            }

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

            //1. creación de doses por defecto.
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                var dosesDefaultId = await CreateDefaultDoses(id);
                relatedIds.Add(new RelatedId
                {
                    EntityIndex = (int)EntityRelated.DOSES,
                    EntityId = dosesDefaultId
                });

            }



            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.PRODUCT,
                    Created = DateTime.Now,
                    RelatedProperties = GetElementRelated(input),
                    RelatedIds = relatedIds.ToArray(),
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
