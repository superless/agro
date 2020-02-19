using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.model.external;
using trifenix.agro.model.external.output;
using trifenix.agro.search.model;

namespace trifenix.agro.external.interfaces.entities.orders
{
    public interface IExecutionOrderOperations <T>{ 

        Task<ExtGetContainer<T>> GetExecutionOrder(string id);

        Task<ExtGetContainer<List<T>>> GetExecutionOrders(string idOrder = null);

        Task<ExtPostContainer<string>> SaveNewExecutionOrder(string idOrder, string executionName, string idUserApplicator, string idNebulizer, string[] idsProduct, double[] quantitiesByHectare, string idTractor, string commentary);

        Task<ExtPostContainer<T>> SaveEditExecutionOrder(string idExecutionOrder, string idOrder, string executionName, string idUserApplicator, string idNebulizer, string[] idsProduct, double[] quantitiesByHectare, string idTractor);

        Task<ExtPostContainer<T>> SetStatus(string idExecutionOrder, string typeOfStatus, int newValueOfStatus, string commentary);

        Task<ExtPostContainer<T>> AddCommentary(string idExecutionOrder, string commentary);

        ExtGetContainer<EntitiesSearchContainer> GetIndexElements(string textToSearch, string abbSpecie, int? status, int? page, int? quantity, bool? desc);
        
        ExtGetContainer<SearchResult<T>> GetPaginatedExecutions(string textToSearch, string abbSpecie, int? status, int? page, int? quantity, bool? desc);

    }
}
