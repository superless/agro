using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro.orders;

namespace trifenix.agro.db.interfaces.agro.orders
{
    public interface IExecutionOrderRepository
    {
        Task<string> CreateUpdateExecutionOrder(ExecutionOrder executionOrder);

        Task<ExecutionOrder> GetExecutionOrder(string id);

        IQueryable<ExecutionOrder> GetExecutionOrders();

        Task<long> Total(string season);
    }
}
