using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.external.interfaces.entities.orders
{
    public interface IApplicationOrderOperations
    {

        Task<ExtPostContainer<string>> SaveNewApplicationOrder(ApplicationOrderInput input);



    }
}
