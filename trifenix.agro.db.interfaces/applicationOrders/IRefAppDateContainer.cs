using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.enforcements.ApplicationOrders;

namespace trifenix.agro.db.interfaces.applicationOrders
{
    public interface IRefAppDateContainer {
        Task<string> CreateUpdateDateAppRef(RefApplicationDate dateApp);

        Task<RefApplicationDate> GeteDateAppRef(string uniqueId);

        IQueryable<RefApplicationDate> GetDateAppRefs();
    }
}
