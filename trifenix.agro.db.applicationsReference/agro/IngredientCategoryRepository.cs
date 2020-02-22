using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class IngredientCategoryRepository : IIngredientCategoryRepository
    {
        private readonly IMainDb<IngredientCategory> _db;

        public IngredientCategoryRepository(IMainDb<IngredientCategory> db) 
        {
            _db = db;
        }

        public async Task<string> CreateUpdateIngredientCategory(IngredientCategory category)
        {
            return await _db.CreateUpdate(category);
        }

        public async Task<IngredientCategory> GetIngredientCategory(string uniqueId)
        {
            return await _db.GetEntity(uniqueId);
        }

        public IQueryable<IngredientCategory> GetIngredientCategories()
        {
            return _db.GetEntities();
        }
    }
}
