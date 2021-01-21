using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.connect.interfaces.db.cosmos;

namespace trifenix.connect.agro.interfaces.cosmos
{
    public interface IDbExistsElements : IExistElement
    {
        Task<bool> ExistsDosesFromOrder(string idDoses);

        Task<bool> ExistsDosesExecutionOrder(string idDoses);

    }
}
