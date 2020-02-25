using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.model.external.output;
using trifenix.agro.search.model;

namespace trifenix.agro.external.interfaces.entities.orders {
    public interface IApplicationOrderOperations {

        Task<ExtPostContainer<string>> SaveNewApplicationOrder(ApplicationOrderInput input);

        Task<ExtPostContainer<OutPutApplicationOrder>> SaveEditApplicationOrder(string id, ApplicationOrderInput input);

        Task<ExtGetContainer<OutPutApplicationOrder>> GetApplicationOrder(string id);

        Task<ExtGetContainer<List<OutPutApplicationOrder>>> GetApplicationOrders();

        ExtGetContainer<EntitiesSearchContainer> GetIndexElements(string textToSearch, string abbSpecie, bool? type, int? page, int? quantity, bool? desc);

        ExtGetContainer<SearchResult<OutPutApplicationOrder>> GetPaginatedOrders(string textToSearch, string abbSpecie, bool? type, int? page, int? quantity, bool? desc);

    }
}