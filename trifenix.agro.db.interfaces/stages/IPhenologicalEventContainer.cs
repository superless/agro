using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.enforcements.stages;

namespace trifenix.agro.db.interfaces.stages
{
    public interface IPhenologicalEventContainer
    {
        Task<string> CreateUpdatePhenologicalEvent(PhenologicalEvent phenologicalEvent);

        Task<PhenologicalEvent> GetPhenologicalEvent(string uniqueId);

        IQueryable<PhenologicalEvent> GetPhenologicalEvents();

    }
}
