using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.model.external;
using trifenix.agro.model.external.output;
using trifenix.agro.search.model;

namespace trifenix.agro.external.interfaces.entities.orders
{
    public interface IExecutionOrderOperations{ 

        Task<ExtGetContainer<ExecutionOrder>> GetExecutionOrder(string id);

        

        Task<ExtPostContainer<string>> SaveNewExecutionOrder(string idOrder, string idUserApplicator, string idNebulizer, string[] idsProduct, double[] quantitiesByHectare, string idTractor, string commentary);

        Task<ExtPostContainer<string>> SaveEditExecutionOrder(string idExecutionOrder, string idOrder, string idUserApplicator, string idNebulizer, string[] idsProduct, double[] quantitiesByHectare, string idTractor);

        Task<ExtPostContainer<ExecutionOrder>> SetStatus(string idExecutionOrder, string typeOfStatus, int newValueOfStatus, string commentary);

    }
}
