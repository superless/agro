using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro
{
    public interface IIngredientCategoryRepository
    {
        Task<string> CreateUpdateIngredientCategory(IngredientCategory category);

        Task<IngredientCategory> GetIngredientCategory(string uniqueId);

        IQueryable<IngredientCategory> GetIngredientCategories();

    }
}