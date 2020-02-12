using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro.core;

namespace trifenix.agro.db.interfaces.agro {
    public interface IBusinessNameRepository {

        Task<string> CreateUpdateBusinessName(BusinessName BusinessName);
        Task<BusinessName> GetBusinessName(string id);
        IQueryable<BusinessName> GetBusinessNames();

    }
}