using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main
{
    public class IngredientOperations : MainReadOperationName<Ingredient, IngredientInput>, IGenericOperation<Ingredient, IngredientInput>
    {
        public IngredientOperations(IMainGenericDb<Ingredient> repo, IExistElement existElement, IAgroSearch search) : base(repo, existElement, search)
        {
        }

        public async Task<ExtPostContainer<string>> Save(IngredientInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var ingredient = new Ingredient
            {
                Id = id,
                Name = input.Name,
                idCategory = input.Id
            };
            var valida = await Validate(input);
            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, ingredient.CosmosEntityName));

            var existsCategory = await existElement.ExistsElement<Ingredient>("idCategory", input.idCategory);
            if (!existsCategory) throw new Exception(string.Format(ErrorMessages.NotValidId, "Categoria de Ingrediente"));


            await repo.CreateUpdate(ingredient);

            search.AddSimpleEntities(new List<SimpleSearch>
            {
                new SimpleSearch{
                    Created = DateTime.Now,
                    Id = id,
                    Name = input.Name,
                    EntityName = ingredient.CosmosEntityName
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
