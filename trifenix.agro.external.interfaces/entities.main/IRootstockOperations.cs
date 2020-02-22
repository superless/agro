using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.main
{
    public interface IRootstockOperations
    {
        Task<ExtPostContainer<string>> SaveNewRootstock(string name, string abbreviation);

        Task<ExtPostContainer<Rootstock>> SaveEditRootstock(string id, string name, string abbreviation);

        Task<ExtGetContainer<List<Rootstock>>> GetRootstocks();
        
    }

}
