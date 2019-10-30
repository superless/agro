using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main
{
    public class IngredientOperations : IIngredientsOperations
    {
        private readonly IIngredientRepository _repo;
        private readonly IIngredientCategoryRepository _repoCategory;
        private readonly ICommonDbOperations<Ingredient> _commonDb;
        public IngredientOperations(IIngredientRepository repo, IIngredientCategoryRepository repoCategory, ICommonDbOperations<Ingredient> commonDb)
        {
            _repo = repo;
            _repoCategory = repoCategory;
            _commonDb = commonDb;
        }
        public async Task<ExtGetContainer<List<Ingredient>>> GetIngredients()
        {

            var ingredientsQuery = _repo.GetIngredients();
            var ingredients = await _commonDb.TolistAsync(ingredientsQuery);
            return OperationHelper.GetElements(ingredients);
        }

        public async Task<ExtPostContainer<Ingredient>> SaveEditIngredient(string id, string name, string categoryId)
        {
            try
            {
                var ingredient = await _repo.GetIngredient(id);

                if (ingredient == null)
                {
                    return new ExtPostErrorContainer<Ingredient>
                    {
                        Message = $"No existe ingrediente con id : {id}",
                        MessageResult = ExtMessageResult.ElementToEditDoesNotExists,
                        IdRelated = id
                    };
                }

                var categoryIngredient = await _repoCategory.GetIngredientCategory(categoryId);
                if (categoryIngredient == null)
                {
                    return new ExtPostErrorContainer<Ingredient>
                    {
                        Message = $"No existe categoria con id : {categoryId}",
                        MessageResult = ExtMessageResult.ChildRequiredDoesNotExists,
                        IdRelated = categoryId
                    };
                }



                ingredient.Name = name;

                await _repo.CreateUpdateIngredient(ingredient);

                return new ExtPostContainer<Ingredient>
                {
                    Result = ingredient,
                    IdRelated = id,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception ex)
            {
                return new ExtPostContainer<Ingredient>
                {
                    IdRelated = id,
                    MessageResult = ExtMessageResult.Error,
                    Message = ex.Message
                };

            }
        }

        

        public async Task<ExtPostContainer<string>> SaveNewIngredient(string name, string categoryId)
        {
            try
            {
                var categoryIngredient = await _repoCategory.GetIngredientCategory(categoryId);
                if (categoryIngredient == null)
                {
                    return OperationHelper.PostNotFoundElementException<string>($"No existe categoria con id : {categoryId}", categoryId);

                }

                return await OperationHelper.CreateElement(_commonDb, _repo.GetIngredients(),
                    async s => await _repo.CreateUpdateIngredient(new Ingredient
                    {
                        Id = s,
                        Name = name,
                        Category = categoryIngredient
                    }),
                    s => s.Name.Equals(name),
                    $"ya existe ingrediente activo con nombre {name} "

                );
            }
            catch (Exception e)
            {

                return OperationHelper.GetPostException<string>(e);
            }

            
        }
    }
}
