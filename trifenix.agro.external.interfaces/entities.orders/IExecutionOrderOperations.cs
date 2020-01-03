using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.orders
{
    public interface IExecutionOrderOperations { 

        Task<ExtGetContainer<ExecutionOrder>> GetExecutionOrderOrder(string id);

        Task<ExtGetContainer<List<ExecutionOrder>>> GetExecutionOrderOrders();

        Task<ExtPostContainer<string>> SaveNewExecutionOrder(string idOrder, string idUserApplicator, string idNebulizer, string idTractor, string commentary);

        Task<ExtPostContainer<ExecutionOrder>> AddCommentaryToExecutionOrder(string idExecutionOrder, string commentary);

        Task<ExtPostContainer<ExecutionOrder>> SaveEditExecutionOrder(string id, string idOrder, ExecutionStatus executionStatus, FinishStatus finishStatus, ClosedStatus closedStatus, string idUserApplicator, string idNebulizer, string idTractor);

    }
}
