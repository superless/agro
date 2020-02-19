using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.model.agro.orders;

namespace trifenix.agro.db.applicationsReference.agro.orders {
    public class ExecutionOrderRepository : IExecutionOrderRepository {

        private readonly IMainDb<ExecutionOrder> _db;
        public ExecutionOrderRepository(IMainDb<ExecutionOrder> db) {
            _db = db;
        }

        public async Task<string> CreateUpdateExecutionOrder(ExecutionOrder executionOrder) {
            return await _db.CreateUpdate(executionOrder);
        }

        public async Task<ExecutionOrder> GetExecutionOrder(string id) {
            return await _db.GetEntity(id);
        }

        public IQueryable<ExecutionOrder> GetExecutionOrders(string idOrder = null) {
            var executions = _db.GetEntities();
            if(!string.IsNullOrWhiteSpace(idOrder))
                executions = executions.Where(execution => execution.Order.Id.Equals(idOrder));
            return executions;
        }

        public async Task<long> Total(string season) {
            return await _db.Store.QuerySingleAsync<long>($"SELECT value count(1) FROM c where c.SeasonId = '{season}'");
        }
    }
}
