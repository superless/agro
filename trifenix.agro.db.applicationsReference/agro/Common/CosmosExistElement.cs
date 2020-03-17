using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;

namespace trifenix.agro.db.applicationsReference.agro.Common
{
    public class CosmosExistElement : BaseQueries, IExistElement
    {
        

        public CosmosExistElement(AgroDbArguments dbArguments): base(dbArguments) {
            
        }

        public async Task<bool> ExistsById<T>(string id) where T: DocumentBase
        {   
            return await ExistsCustom<T>(Queries(DbQuery.COUNT_BY_ID),id);
        }

        public async Task<bool> ExistsWithPropertyValue<T>(string namePropCheck, string valueCheck, string id = null) where T : DocumentBase
        {   
            
            if (!string.IsNullOrWhiteSpace(id))
                return await ExistsCustom<T>(Queries(DbQuery.COUNT_BY_NAMEVALUE_AND_NOID), namePropCheck, valueCheck,  id);

            return await ExistsCustom<T>(Queries(DbQuery.COUNT_BY_NAMEVALUE), namePropCheck, valueCheck);
        }

        public async Task<bool> ExistsCustom<T>(string query, params object[] args) where T : DocumentBase
        {   
            var result = await SingleQuery<T, long>(query, args);
            return result != 0;
        }

        public async Task<bool> ExistsDosesFromOrder(string idDoses)
        {
            return await ExistsCustom<ApplicationOrder>(Queries(DbQuery.COUNT_EXECUTION_OR_ORDERS_BY_DOSESID), idDoses);
        }

        public async Task<bool> ExistsDosesExecutionOrder(string idDoses)
        {
            return await ExistsCustom<ExecutionOrder>(Queries(DbQuery.COUNT_EXECUTION_OR_ORDERS_BY_DOSESID), idDoses);
        }

    }
}