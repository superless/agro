using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class ApplicationTargetRepository : IApplicationTargetRepository
    {

        private readonly IMainDb<ApplicationTarget> _db;
        public ApplicationTargetRepository(IMainDb<ApplicationTarget> db) 
        {
            _db = db;
        }

        public async Task<string> CreateUpdateTargetApp(ApplicationTarget target)
        {
            return await _db.CreateUpdate(target);
        }

        public async Task<ApplicationTarget> GetTarget(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<ApplicationTarget> GetTargets()
        {
            return _db.GetEntities();
        }
    }
}
