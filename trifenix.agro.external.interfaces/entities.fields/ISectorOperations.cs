using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.fields
{
    public interface ISectorOperations
    {
        Task<ExtPostContainer<string>> SaveNewSector(string name);

        Task<ExtPostContainer<Sector>> SaveEditSector(string id, string name);

        Task<ExtGetContainer<List<Sector>>> GetSectors();

        Task<ExtGetContainer<Sector>> GetSector(string id);




    }
}
