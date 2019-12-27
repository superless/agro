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
    public class IngredientCategoryOperations : IIngredientCategoryOperations
    {

        private readonly IIngredientCategoryRepository _repo;
        private readonly ICommonDbOperations<IngredientCategory> _commonDb;
        public IngredientCategoryOperations(IIngredientCategoryRepository repo, ICommonDbOperations<IngredientCategory> commonDb)
        {
            _repo = repo;
            _commonDb = commonDb;
        }

        public async Task<ExtGetContainer<List<IngredientCategory>>> GetIngredientCategories()
        {
            try
            {
                var categoriesQuery = _repo.GetIngredientCategories();
                var categories = await _commonDb.TolistAsync(categoriesQuery);
                return OperationHelper.GetElements(categories);
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
            var element = await _repo.GetIngredientCategory(id);
            return await OperationHelper.EditElement(_commonDb, _repo.GetIngredientCategories(), 
                id,
                element,
                s => {
                    s.Name = name;
                    return s;
                },
                _repo.CreateUpdateIngredientCategory,
                 $"No existe categoria con id : {id}",
                s => s.Name.Equals(name),
                $"Ya existe categoria con nombre: {name}"
            );
        }

        public async Task<ExtPostContainer<string>> SaveNewIngredientCategory(string name)
        {
            return await OperationHelper.CreateElement(_commonDb, _repo.GetIngredientCategories(),
                async s => await _repo.CreateUpdateIngredientCategory(new IngredientCategory
                {
                    Id = s,
                    Name = name
                }),
                s => s.Name.Equals(name),
                $"Ya existe categoria con nombre: {name}"

            );
        }
    }
}
