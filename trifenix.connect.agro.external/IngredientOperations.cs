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

namespace trifenix.agro.external.operations.entities.main
{

    public class IngredientOperations<T> : MainOperation<Ingredient, IngredientInput, T>, IGenericOperation<Ingredient, IngredientInput> {

        public IngredientOperations(IMainGenericDb<Ingredient> repo, IAgroSearch<T> search, ICommonDbOperations<Ingredient> commonDb, IValidatorAttributes<IngredientInput, Ingredient> validator) : base(repo, search, commonDb, validator) {}

        public Task Remove(string id) {
            throw new NotImplementedException();
        }

        public async Task<ExtPostContainer<string>> Save(Ingredient ingredient) {
            await repo.CreateUpdate(ingredient);
            search.AddDocument(ingredient);
            return new ExtPostContainer<string> {
                IdRelated = ingredient.Id,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<string>> SaveInput(IngredientInput input, bool isBatch) {
            await Validate(input);
            var id = !string.IsNullOrWhiteSpace(input.Id) ? input.Id : Guid.NewGuid().ToString("N");
            var ingredient = new Ingredient {
                Id = id,
                Name = input.Name,
                idCategory = input.idCategory
            };
            if (!isBatch)
                return await Save(ingredient);
            await repo.CreateEntityContainer(ingredient);
            return new ExtPostContainer<string> {
                IdRelated = id,
                MessageResult = ExtMessageResult.Ok
            };
        }

    }

}