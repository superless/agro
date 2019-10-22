using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro.fields
{
    public class SectorRepository : MainDb<Sector>, ISectorRepository
    {
        public SectorRepository(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdateSector(Sector sector)
        {
            return await CreateUpdate(sector);
        }

        public async Task<Sector> GetSector(string id)
        {
            return await GetEntity(id);
        }

        public IQueryable<Sector> GetSectors()
        {
            return GetEntities();
        }
    }
}
