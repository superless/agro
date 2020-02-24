using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.applicationsReference.agro.Common;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;

namespace trifenix.agro.db.applicationsReference.agro.orders {
    public class ExecutionOrderRepository : IExecutionOrderRepository {

        private readonly IMainDb<ExecutionOrder> _db;
        private readonly AgroDbArguments _dbArguments;

        public ExecutionOrderRepository(AgroDbArguments dbArguments)
        {
            _db = new MainDb<ExecutionOrder>(dbArguments);

            _dbArguments = dbArguments;
        }

        public async Task<string> CreateUpdateExecutionOrder(ExecutionOrder executionOrder) {
            return await _db.CreateUpdate(executionOrder);
        }

        
        
        public async Task<ExecutionOrder> GetExecutionOrder(string id) {
            return await _db.GetEntity(id);
        }


      
    }
}
