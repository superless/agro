using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro.core;

namespace trifenix.agro.db.applicationsReference.agro {
    public class CostCenterRepository : ICostCenterRepository {

        private readonly IMainDb<CostCenter> _db;
        public CostCenterRepository(IMainDb<CostCenter> db) {
            _db = db;
        }

        public async Task<string> CreateUpdateCostCenter(CostCenter CostCenter) {
            return await _db.CreateUpdate(CostCenter);
        }

        public async Task<CostCenter> GetCostCenter(string id) {
            return await _db.GetEntity(id);
        }

        public IQueryable<CostCenter> GetCostCenters() {
            return _db.GetEntities();
        }

    }
}