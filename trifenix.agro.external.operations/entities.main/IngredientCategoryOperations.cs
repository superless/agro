using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main
{
    public class IngredientCategoryOperations : MainReadOperationName<IngredientCategory, IngredientCategoryInput>, IGenericOperation<IngredientCategory, IngredientCategoryInput>
    {
        public IngredientCategoryOperations(IMainGenericDb<IngredientCategory> repo, IExistElement existElement, IAgroSearch search) : base(repo, existElement, search)
        {
        }

        public async Task<ExtPostContainer<string>> Save(IngredientCategoryInput input)
        {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");

            var category = new IngredientCategory
            {
                Id = id,
                Name = input.Name
            };
            var valida = await Validate(input);
            if (!valida) throw new Exception(string.Format(ErrorMessages.NotValid, category.CosmosEntityName));

            await repo.CreateUpdate(category);

            search.AddElements(new List<EntitySearch>
            {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.CATEGORY_INGREDIENT,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = input.Name
                        }
                    },
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
