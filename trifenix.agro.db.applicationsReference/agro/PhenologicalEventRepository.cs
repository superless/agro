using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class PhenologicalEventRepository : IPhenologicalEventRepository
    {

        private readonly IMainDb<Event> _db;
        public PhenologicalEventRepository(IMainDb<Event> db)
        {
            _db = db;
        }

        public async Task<string> CreateUpdatePhenologicalEvent(Event phenologicalEvent)
        {
            return await _db.CreateUpdate(phenologicalEvent);
        }

        public async Task<Event> GetPhenologicalEvent(string uniqueId)
        {
            return await _db.GetEntity(uniqueId);
        }

        public IQueryable<Event> GetPhenologicalEvents()
        {
            return _db.GetEntities();
        }
    }
}
