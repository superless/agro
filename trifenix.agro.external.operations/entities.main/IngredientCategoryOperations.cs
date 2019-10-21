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
    public class IngredientCategoryOperations : IIngredientCategoryOperations
    {

        private readonly IIngredientCategoryRepository _repo;
        public IngredientCategoryOperations(IIngredientCategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<ExtGetContainer<List<IngredientCategory>>> GetIngredientCategories()
        {
            try
            {
                var elements = await _repo.GetIngredientCategories().ToListAsync();
                return new ExtGetContainer<List<IngredientCategory>>
                {
                    Result = elements,
                    StatusResult = elements.Any() ? ExtGetDataResult.Success : ExtGetDataResult.EmptyResults
                };
            }
            catch (Exception ex)
            {

                return new ExtGetErrorContainer<List<IngredientCategory>>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = ex.Message,
                    InternalException = ex
                };
            }
        }

        public async Task<ExtPostContainer<IngredientCategory>> SaveEditIngredientCategory(string id, string name)
        {
            try
            {
                var category = await _repo.GetIngredientCategory(id);

                if (category == null)
                {
                    return new ExtPostErrorContainer<IngredientCategory>
                    {
                        Message = $"No existe categoria de ingredientes con id : {id}",
                        MessageResult = ExtMessageResult.ElementToEditDoesNotExists,
                        IdRelated = id
                    };
                }

                category.Name = name;

                await _repo.CreateUpdateIngredientCategory(category);

                return new ExtPostContainer<IngredientCategory>
                {
                    Result = category,
                    IdRelated = id,
                    MessageResult = ExtMessageResult.Ok
                };

            }
            catch (Exception ex)
            {

                return new ExtPostContainer<IngredientCategory>
                {
                    IdRelated = id,
                    MessageResult = ExtMessageResult.Error,
                    Message = ex.Message
                };
            }
        }

        public async Task<ExtPostContainer<string>> SaveNewIngredientCategory(string name)
        {
            try
            {
                var idResult = await _repo.CreateUpdateIngredientCategory(new IngredientCategory
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = name
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
