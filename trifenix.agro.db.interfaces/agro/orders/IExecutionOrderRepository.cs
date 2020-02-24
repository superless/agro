using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;

namespace trifenix.agro.db.interfaces.agro.orders
{
    public interface IExecutionOrderRepository
    {
        Task<string> CreateUpdateExecutionOrder(ExecutionOrder executionOrder);

        Task<ExecutionOrder> GetExecutionOrder(string id);


        

        
    }
}
