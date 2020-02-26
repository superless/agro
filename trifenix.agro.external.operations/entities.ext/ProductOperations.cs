using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
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

        public ProductOperations(IMainGenericDb<Product> repo, IExistElement existElement, IAgroSearch search, IGenericOperation<Doses, DosesInput> dosesOperation) : base(repo, existElement, search)
        {
            this.dosesOperation = dosesOperation;
        }

        private IdsRelated[] GetIdsRelated(ProductInput input) {

            var list = new List<IdsRelated>();
            if (!string.IsNullOrWhiteSpace(input.IdActiveIngredient))
            {
                list.Add(new IdsRelated { EntityIndex = (int)EntityRelated.INGREDIENT, EntityId = input.IdActiveIngredient });
            }
            return list.ToArray();
        }

        private ElementRelated[] GetElementRelated(ProductInput input) {
            var list = new List<ElementRelated>();
            if (!string.IsNullOrWhiteSpace(input.Brand))
            {
                list.Add(new ElementRelated { EntityIndex = (int)PropertyRelated.PRODUCT_BRAND, Name = input.Brand });
            }

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
            var existsIngredient = await existElement.ExistsElement<Ingredient>(productInput.IdActiveIngredient);

            if (!existsIngredient) return "no existe id de ingrediente";

            return string.Empty;


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

            await repo.CreateUpdate(product);

            if (input.Doses!= null && input.Doses.Any())
            {
                foreach (var dose in input.Doses)
                {
                    dose.IdProduct = id;
                    await dosesOperation.Save(dose);
                }
            }

            search.AddEntities(new List<EntitySearch>
            {
                new EntitySearch{
                    Created = DateTime.Now,
                    Id = id,
                    ElementsRelated = GetElementRelated(input),
                    IdsRelated = GetIdsRelated(input),
                    Name = input.Name,
                    EntityName = product.CosmosEntityName
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
