using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class IngredientCategoryRepository : MainDb<IngredientCategory>, IIngredientCategoryRepository
    {
        public IngredientCategoryRepository(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdateIngredientCategory(IngredientCategory category)
        {
            return await CreateUpdate(category);
        }

        public async Task<IngredientCategory> GetIngredientCategory(string uniqueId)
        {
            return await GetEntity(uniqueId);
        }

        public IQueryable<IngredientCategory> GetIngredientCategories()
        {
            return GetEntities();
        }
    }
}
