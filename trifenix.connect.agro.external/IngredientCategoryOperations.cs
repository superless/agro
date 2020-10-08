using System;
using System.Threading.Tasks;
using trifenix.connect.agro.external.main;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.external
{
    public class IngredientCategoryOperations<T> : MainOperation<IngredientCategory, IngredientCategoryInput, T>, IGenericOperation<IngredientCategory, IngredientCategoryInput> {
        public IngredientCategoryOperations(IMainGenericDb<IngredientCategory> repo, IAgroSearch<T> search, ICommonDbOperations<IngredientCategory> commonDb, IValidatorAttributes<IngredientCategoryInput, IngredientCategory> validator) : base(repo, search, commonDb, validator) { }


        //TODO : remover ingrediente.
        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(IngredientCategory ingredientCategory) {
            await repo.CreateUpdate(ingredientCategory);
            search.AddDocument(ingredientCategory);

            return new ExtPostContainer<string> {
                IdRelated = ingredientCategory.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(IngredientCategoryInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var category = new IngredientCategory {
                Id = id,
                Name = input.Name
            };
            if (!isBatch)
                return await Save(category);
            await repo.CreateEntityContainer(category);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}