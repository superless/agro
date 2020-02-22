using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model;

namespace trifenix.agro.db.interfaces.agro.fields
{
    public interface ISectorRepository
    {
        Task<string> CreateUpdateSector(Sector sector);

        Task<Sector> GetSector(string id);

        IQueryable<Sector> GetSectors();
            
    }
}
