using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations.res;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.main {

    public class IngredientOperations : MainOperation<Ingredient>, IGenericOperation<Ingredient, IngredientInput> {

        public IngredientOperations(IMainGenericDb<Ingredient> repo, IExistElement existElement, IAgroSearch search, ICommonDbOperations<Ingredient> commonDb) : base(repo, existElement, search, commonDb) {}

        public async Task<ExtPostContainer<string>> Save(IngredientInput input) {
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var ingredient = new Ingredient {
                Id = id,
                Name = input.Name,
                idCategory = input.idCategory
            };
            var valida = await Validate(input);
            if (!valida)
                throw new Exception(string.Format(ErrorMessages.NotValid, ingredient.CosmosEntityName));
            var existsCategory = await existElement.ExistsWithPropertyValue<Ingredient>("idCategory", input.idCategory);
            if (!existsCategory)
                throw new Exception(string.Format(ErrorMessages.NotValidId, "Categoria de Ingrediente"));
            await repo.CreateUpdate(ingredient);
            search.AddElements(new List<EntitySearch> {
                new EntitySearch{
                    Id = id,
                    EntityIndex = (int)EntityRelated.INGREDIENT,
                    Created = DateTime.Now,
                    RelatedProperties = new Property[] {
                        new Property {
                            PropertyIndex = (int)PropertyRelated.GENERIC_NAME,
                            Value = input.Name
                        }
                    },
                    RelatedIds = new RelatedId[]{
                        new RelatedId{
                            EntityIndex = (int)EntityRelated.CATEGORY_INGREDIENT,
                            EntityId = input.idCategory
                        }
                    }
                }
            });
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok,
                Result = id
            };
        }

    }

}