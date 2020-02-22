using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model;

namespace trifenix.agro.db.applicationsReference.agro.fields
{
    public class SectorRepository : ISectorRepository
    {

        private readonly IMainDb<Sector> _db;

        public SectorRepository(IMainDb<Sector> db) 
        {
            _db = db;
        }

        public async Task<string> CreateUpdateSector(Sector sector)
        {
            return await _db.CreateUpdate(sector);
        }

        public async Task<Sector> GetSector(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<Sector> GetSectors()
        {
            return _db.GetEntities();
        }
    }
}
