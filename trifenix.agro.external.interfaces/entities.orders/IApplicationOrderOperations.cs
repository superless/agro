using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.external.interfaces.entities.orders
{
    public interface IApplicationOrderOperations
    {

        Task<ExtPostContainer<string>> SaveNewApplicationOrder(ApplicationOrderInput input);

        Task<ExtPostContainer<ApplicationOrder>> SaveEditPhenologicalPreOrder(string id, ApplicationOrderInput input);

        Task<ExtGetContainer<ApplicationOrder>> GetApplicationOrder(string id);

        Task<ExtGetContainer<List<ApplicationOrder>>> GetApplicationOrders();



    }
}
