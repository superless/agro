using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro
{
    public  interface IPhenologicalEventRepository
    {
        Task<string> CreateUpdatePhenologicalEvent(PhenologicalEvent phenologicalEvent);

        Task<PhenologicalEvent> GetPhenologicalEvent(string uniqueId);

        IQueryable<PhenologicalEvent> GetPhenologicalEvents();
    }
}
