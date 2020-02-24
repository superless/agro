using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.fields
{
    public interface ISectorOperations
    {
        Task<ExtPostContainer<string>> SaveNewSector(string name);

        Task<ExtPostContainer<string>> SaveEditSector(string id, string name);
        Task<ExtGetContainer<Sector>> GetSector(string id);
    }


}
