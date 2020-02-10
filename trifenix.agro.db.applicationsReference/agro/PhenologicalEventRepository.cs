using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class PhenologicalEventRepository : IPhenologicalEventRepository
    {

        private readonly IMainDb<PhenologicalEvent> _db;
        public PhenologicalEventRepository(IMainDb<PhenologicalEvent> db)
        {
            _db = db;
        }

        public async Task<string> CreateUpdatePhenologicalEvent(PhenologicalEvent phenologicalEvent)
        {
            return await _db.CreateUpdate(phenologicalEvent);
        }

        public async Task<PhenologicalEvent> GetPhenologicalEvent(string uniqueId)
        {
            return await _db.GetEntity(uniqueId);
        }

        public IQueryable<PhenologicalEvent> GetPhenologicalEvents()
        {
            return _db.GetEntities();
        }
    }
}
