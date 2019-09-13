using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.enforcements.ApplicationOrders;

namespace trifenix.agro.db.interfaces.applicationOrders
{
    public interface IRefAppPhenologicalContainer {

        Task<string> CreateUpdatePhenologicalAppRef(RefApplicaByPhenologicalEvent phenologicalApp);


        Task<RefApplicaByPhenologicalEvent> GetPhenologicalAppRef(string uniqueId);

        IQueryable<RefApplicaByPhenologicalEvent> GetPhenologicalAppRefs();
    }
    


}
