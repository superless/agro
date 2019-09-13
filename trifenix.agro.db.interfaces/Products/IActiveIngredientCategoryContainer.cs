using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.enforcements.products;

namespace trifenix.agro.db.interfaces.Products
{
    public  interface IActiveIngredientCategoryContainer
    {
        Task<string> CreateUpdateCategory(ActiveIngredientCategory category);

        Task<ActiveIngredientCategory> GetCategory(string uniqueId);

        IQueryable<ActiveIngredientCategory> GetCategories();
    }
}
