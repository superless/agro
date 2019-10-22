using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro.orders
{
    public interface IPhenologicalPreOrderRepository
    {
        Task<string> CreateUpdatePhenologicalPreOrder(PhenologicalPreOrder preOrder);

        Task<PhenologicalPreOrder> GetPhenologicalPreOrder(string id);

        IQueryable<PhenologicalPreOrder> GetPhenologicalPreOrders();
    }
}
