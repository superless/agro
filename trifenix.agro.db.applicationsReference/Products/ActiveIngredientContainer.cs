using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.Products;
using trifenix.agro.db.model.enforcements.products;

namespace trifenix.agro.db.applicationsReference.Products
{
    public class ActiveIngredientContainer : MainDb<ActiveIngredient>, IActiveIngredientContainer
    {
        public ActiveIngredientContainer(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdateActiveIngredient(ActiveIngredient activeIngredient)
        {
            return await CreateUpdate(activeIngredient);
        }

        public async Task<ActiveIngredient> GetActiveIngredient(string uniqueId)
        {
            return await GetEntity(uniqueId);
        }

        public IQueryable<ActiveIngredient> GetActiveIngredients()
        {
            return GetEntities();
        }
    }
}
