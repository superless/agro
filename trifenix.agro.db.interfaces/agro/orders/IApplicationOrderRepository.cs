using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro.orders;

namespace trifenix.agro.db.interfaces.agro.orders
{
    public interface IApplicationOrderRepository
    {
        Task<string> CreateUpdate(ApplicationOrder order);

        Task<ApplicationOrder> GetApplicationOrder(string id);

        IQueryable<ApplicationOrder> GetApplicationOrders();

        Task<long> Total(string season);
    }
}
