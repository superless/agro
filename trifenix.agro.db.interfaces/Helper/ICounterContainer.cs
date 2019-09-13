using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace trifenix.agro.db.interfaces.Helper
{
    public interface ICounterContainer
    {

        Task<long> GetNextTaskId();
    }
}
