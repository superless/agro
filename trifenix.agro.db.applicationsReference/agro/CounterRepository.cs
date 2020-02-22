using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model;

namespace trifenix.agro.db.applicationsReference.agro {
    public class CounterRepository : ICounterRepository {

        private readonly IMainDb<Counter> _db;
        public CounterRepository(IMainDb<Counter> db) {
            _db = db;
        }

        public async Task<string> CreateUpdateCounter(Counter Counter) {
            return await _db.CreateUpdate(Counter);
        }

        public Counter GetCounter() {
            return _db.GetEntities().FirstOrDefault();
        }

    }
}