using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.model.external;
using trifenix.agro.model.external.output;
using trifenix.agro.search.model;

namespace trifenix.agro.external.interfaces.entities.orders
{
    public interface IExecutionOrderOperations { 

        Task<ExtGetContainer<ExecutionOrder>> GetExecutionOrder(string id);

        Task<ExtGetContainer<List<ExecutionOrder>>> GetExecutionOrders();

        Task<ExtPostContainer<string>> SaveNewExecutionOrder(string idOrder, string executionName, string idUserApplicator, string idNebulizer, string[] idProduct, double[] quantityByHectare, string idTractor, string commentary);

        Task<ExtPostContainer<ExecutionOrder>> SaveEditExecutionOrder(string id, string executionName, string idOrder, string idUserApplicator, string idNebulizer, string[] idProduct, double[] quantityByHectare, string idTractor);

        Task<ExtPostContainer<ExecutionOrder>> SetStatus(string idExecutionOrder, string typeOfStatus, int newValueOfStatus, string commentary);

        Task<ExtPostContainer<ExecutionOrder>> AddCommentaryToExecutionOrder(string idExecutionOrder, string commentary);

        ExtGetContainer<EntitiesSearchContainer> GetIndexElements(string textToSearch, int? status, int? page, int? quantity, bool? desc);
        
        ExtGetContainer<SearchResult<ExecutionOrder>> GetPaginatedExecutions(string textToSearch, int? status, int? page, int? quantity, bool? desc);

    }
}
