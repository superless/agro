using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.enforcements.ApplicationOrders;

namespace trifenix.agro.db.interfaces.applicationOrders
{
    public interface IRefAppContinuedContainer {
        Task<string> CreateUpdateContinuedAppRef(RefApplicationContinued continuedApp);

        Task<RefApplicationContinued> GetContinuedAppRef(string uniqueId);

        IQueryable<RefApplicationContinued> GetContinuedAppRefs();
    }
}
