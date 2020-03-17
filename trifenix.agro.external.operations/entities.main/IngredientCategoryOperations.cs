using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main {
    public class IngredientCategoryOperations : MainOperation<IngredientCategory, IngredientCategoryInput>, IGenericOperation<IngredientCategory, IngredientCategoryInput> {
        public IngredientCategoryOperations(IMainGenericDb<IngredientCategory> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<IngredientCategory> commonDb) : base(repo, existElement, search, commonDb) { }

        public async Task<ExtPostContainer<string>> Save(IngredientCategory ingredientCategory) {
            await repo.CreateUpdate(ingredientCategory, false);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = ingredientCategory.Id,
                    EntityIndex = (int)EntityRelated.CATEGORY_INGREDIENT,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = ingredientCategory.Name
                        }
                    },
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = ingredientCategory.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(IngredientCategoryInput input, bool isBatch) {
            await Validate(input, isBatch);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var category = new IngredientCategory {
                Id = id,
                Name = input.Name
            };
            if (!isBatch)
                return await Save(category);
            await repo.CreateUpdate(category, true);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}