using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro.core;

namespace trifenix.agro.db.interfaces.agro {
    public interface ICostCenterRepository {

        Task<string> CreateUpdateCostCenter(CostCenter CostCenter);
        Task<CostCenter> GetCostCenter(string id);
        IQueryable<CostCenter> GetCostCenters();

    }
}