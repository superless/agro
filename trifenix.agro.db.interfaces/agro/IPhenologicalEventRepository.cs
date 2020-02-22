using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model;

namespace trifenix.agro.db.interfaces.agro
{
    public  interface IPhenologicalEventRepository
    {
        Task<string> CreateUpdatePhenologicalEvent(PhenologicalEvent phenologicalEvent);

        Task<PhenologicalEvent> GetPhenologicalEvent(string uniqueId);

        IQueryable<PhenologicalEvent> GetPhenologicalEvents();
    }
}
