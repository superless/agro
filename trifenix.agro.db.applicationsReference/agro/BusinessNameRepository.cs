using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro.core;

namespace trifenix.agro.db.applicationsReference.agro {
    public class BusinessNameRepository : IBusinessNameRepository {

        private readonly IMainDb<BusinessName> _db;
        public BusinessNameRepository(IMainDb<BusinessName> db)  {
            _db = db;
        }

        public async Task<string> CreateUpdateBusinessName(BusinessName BusinessName) {
            return await _db.CreateUpdate(BusinessName);
        }

        public async Task<BusinessName> GetBusinessName(string id) {
            return await _db.GetEntity(id);
        }

        public IQueryable<BusinessName> GetBusinessNames() {
            return _db.GetEntities();
        }

    }
}