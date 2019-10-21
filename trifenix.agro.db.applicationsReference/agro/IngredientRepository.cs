using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class IngredientRepository : MainDb<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdateIngredient(Ingredient ingredient)
        {
            return await CreateUpdate(ingredient);
        }

        public async Task<Ingredient> GetIngredient(string id)
        {
            return await GetIngredient(id);
        }

        public IQueryable<Ingredient> GetIngredients()
        {
            return GetEntities();
        }
    }
}
