using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class TractorRepository : ITractorRepository
    {

        private readonly IMainDb<Tractor> _db;
        public TractorRepository(IMainDb<Tractor> db) 
        {
            _db = db;
        }

        public async Task<string> CreateUpdateTractor(Tractor Tractor)
        {
            return await _db.CreateUpdate(Tractor);
        }

        public async Task<Tractor> GetTractor(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<Tractor> GetTractors()
        {
            return _db.GetEntities();
        }
    }
}
