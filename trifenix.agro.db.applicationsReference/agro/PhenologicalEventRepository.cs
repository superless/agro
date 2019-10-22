using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class PhenologicalEventRepository : MainDb<PhenologicalEvent>, IPhenologicalEventRepository
    {
        public PhenologicalEventRepository(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdatePhenologicalEvent(PhenologicalEvent phenologicalEvent)
        {
            return await CreateUpdate(phenologicalEvent);
        }

        public async Task<PhenologicalEvent> GetPhenologicalEvent(string uniqueId)
        {
            return await GetEntity(uniqueId);
        }

        public IQueryable<PhenologicalEvent> GetPhenologicalEvents()
        {
            return GetEntities();
        }
    }
}
