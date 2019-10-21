using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.main;
using trifenix.agro.model.external;

namespace trifenix.agro.external.operations.entities.main
{
    public class IngredientOperations : IIngredientsOperations
    {
        private readonly IIngredientRepository _repo;
        private readonly IIngredientCategoryRepository _repoCategory;

        public IngredientOperations(IIngredientRepository repo, IIngredientCategoryRepository repoCategory)
        {
            _repo = repo;
            _repoCategory = repoCategory;
        }
        public async Task<ExtGetContainer<List<Ingredient>>> GetIngredients()
        {
            try
            {
                var elements = await _repo.GetIngredients().ToListAsync();

                return new ExtGetContainer<List<Ingredient>>
                {
                    Result = elements,
                    StatusResult = elements.Any() ? ExtGetDataResult.Success : ExtGetDataResult.EmptyResults
                };

            }
            catch (Exception exception)
            {
                return new ExtGetErrorContainer<List<Ingredient>>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = exception.Message,
                    InternalException = exception
                };
            }
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
                    return new ExtPostErrorContainer<string>
                    {
                        Message = $"No existe categoria con id : {categoryId}",
                        MessageResult = ExtMessageResult.ChildRequiredDoesNotExists,
                        IdRelated = categoryId
                    };
                }

                var idResult = await _repo.CreateUpdateIngredient(new Ingredient
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = name,
                    Category = categoryIngredient
                });
                return new ExtPostContainer<string>
                {
                    IdRelated = idResult,
                    Result = idResult,
                    MessageResult = ExtMessageResult.Ok
                };


            }
            catch (Exception ex)
            {
                return new ExtPostErrorContainer<string>
                {
                    InternalException = ex,
                    Message = ex.Message,
                    MessageResult = ExtMessageResult.Error
                };
            }
        }
    }
}
