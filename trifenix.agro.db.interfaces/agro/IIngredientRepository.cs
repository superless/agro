using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro
{
    public interface IIngredientRepository
    {
        Task<string> CreateUpdateIngredient(Ingredient ingredient);

        Task<Ingredient> GetIngredient(string id);

        IQueryable<Ingredient> GetIngredients();

    }
}
