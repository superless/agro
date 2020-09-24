using Microsoft.Spatial;
using System;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.interfaces;
using trifenix.agro.search.interfaces;
using trifenix.agro.validator.interfaces;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.agro.external.operations.entities.main
{
    public class IngredientCategoryOperations<T> : MainOperation<IngredientCategory, IngredientCategoryInput, T>, IGenericOperation<IngredientCategory, IngredientCategoryInput> {
        public IngredientCategoryOperations(IMainGenericDb<IngredientCategory> repo, IExistElement existElement, IAgroSearch<T> search, ICommonDbOperations<IngredientCategory> commonDb, IValidator validators) : base(repo, existElement, search, commonDb, validators) { }


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