using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro.orders
{
    public class PhenologicalPreOrderRepository : MainDb<PhenologicalPreOrder>, IPhenologicalPreOrderRepository
    {
        public PhenologicalPreOrderRepository(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdatePhenologicalPreOrder(PhenologicalPreOrder preOrder)
        {
            return await CreateUpdate(preOrder);
        }

        public async Task<PhenologicalPreOrder> GetPhenologicalPreOrder(string id)
        {
            return await GetEntity(id);
        }

        public IQueryable<PhenologicalPreOrder> GetPhenologicalPreOrders()
        {
            return GetEntities();
        }
    }
}
