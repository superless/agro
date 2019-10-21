using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.main
{
    public interface IApplicationTargetOperations
    {
        Task<ExtPostContainer<string>> SaveNewApplicationTarget(string name);

        Task<ExtPostContainer<ApplicationTarget>> SaveEditApplicationTarget(string id, string name);

        Task<ExtGetContainer<List<ApplicationTarget>>> GetAplicationsTarget();

        


    }
}
