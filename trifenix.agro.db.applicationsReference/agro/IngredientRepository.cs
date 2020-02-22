using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly IMainDb<Ingredient> _db;

        public IngredientRepository(IMainDb<Ingredient> db) 
        {
            _db = db;
        }

        public async Task<string> CreateUpdateIngredient(Ingredient ingredient)
        {
            return await _db.CreateUpdate(ingredient);
        }

        public async Task<Ingredient> GetIngredient(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<Ingredient> GetIngredients()
        {
            return _db.GetEntities();
        }
    }
}
