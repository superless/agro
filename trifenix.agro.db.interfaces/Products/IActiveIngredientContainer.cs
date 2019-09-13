using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.enforcements.products;

namespace trifenix.agro.db.interfaces.Products
{
     public interface IActiveIngredientContainer
    {
        Task<string> CreateUpdateActiveIngredient(ActiveIngredient activeIngredient);

        Task<ActiveIngredient> GetActiveIngredient(string uniqueId);

        IQueryable<ActiveIngredient> GetActiveIngredients();

    }
}
