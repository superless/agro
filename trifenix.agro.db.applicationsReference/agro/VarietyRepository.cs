using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class VarietyRepository : IVarietyRepository
    {

        private readonly IMainDb<Variety> _db;
        public VarietyRepository(IMainDb<Variety> db)
        {
            _db = db;
        }

        public async Task<string> CreateUpdateVariety(Variety variety)
        {
            return await _db.CreateUpdate(variety);
        }

        public IQueryable<Variety> GetVarieties()
        {
            return _db.GetEntities();
        }

        public async Task<Variety> GetVariety(string id)
        {
            return await _db.GetEntity(id);
        }
    }
}
