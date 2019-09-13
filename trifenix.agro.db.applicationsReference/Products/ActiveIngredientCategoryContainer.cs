using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.Products;
using trifenix.agro.db.model.enforcements.products;

namespace trifenix.agro.db.applicationsReference.Products
{
    public class ActiveIngredientCategoryContainer : MainDb<ActiveIngredientCategory>, IActiveIngredientCategoryContainer
    {
        

        public ActiveIngredientCategoryContainer(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdateCategory(ActiveIngredientCategory category)
        {
            return await CreateUpdate(category);
        }

        public IQueryable<ActiveIngredientCategory> GetCategories()
        {
            return GetEntities();
        }

        public async Task<ActiveIngredientCategory> GetCategory(string uniqueId)
        {
            return await GetEntity(uniqueId);
        }
    }
}
