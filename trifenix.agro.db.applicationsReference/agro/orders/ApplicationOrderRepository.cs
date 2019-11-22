using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.model.agro.orders;

namespace trifenix.agro.db.applicationsReference.agro.orders
{
    public class ApplicationOrderRepository : IApplicationOrderRepository
    {
        private readonly IMainDb<ApplicationOrder> _db;
        public ApplicationOrderRepository(IMainDb<ApplicationOrder> db)
        {
            _db = db;
        }

        public async Task<string> CreateUpdate(ApplicationOrder order)
        {
            return await _db.CreateUpdate(order);
        }

        public async Task<ApplicationOrder> GetApplicationOrder(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<ApplicationOrder> GetApplicationOrders()
        {
            return _db.GetEntities();
        }
    }
}
