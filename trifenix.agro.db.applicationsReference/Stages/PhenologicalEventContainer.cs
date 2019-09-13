using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.stages;
using trifenix.agro.db.model.enforcements.stages;

namespace trifenix.agro.db.applicationsReference.Stages
{
    public class PhenologicalEventContainer : MainDb<PhenologicalEvent>, IPhenologicalEventContainer
    {
        public PhenologicalEventContainer(AgroDbArguments args) : base(args)
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
