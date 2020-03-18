using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;

namespace trifenix.agro.db.applicationsReference.agro.Common {

    public class CosmosExistElement : BaseQueries, IExistElement {

        public CosmosExistElement(AgroDbArguments dbArguments): base(dbArguments) { }

// <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Agro-Dev
        public async Task<bool> ExistsById<T>(string id) where T: DocumentBase
        {   
            return await ExistsCustom<T>(Queries(DbQuery.COUNT_BY_ID),id);
// ========================================================================
//         public async Task<bool> ExistsById<T>(string id, bool checkBatch) where T : DocumentBase {
//             var db = new MainGenericDb<T>(DbArguments);
//             long result = 0;
//             string query;
//             if (checkBatch) {
//                 query = $"SELECT value count(1) FROM c where c.Entity.Id = '{id}'";
//                 result += await db.BatchStore.QuerySingleAsync<long>(query);
//             }
//             query = $"SELECT value count(1) FROM c where c.id = '{id}'";
//             result += await db.Store.QuerySingleAsync<long>(query);
//             return result != 0;
// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Script_PoblarDB
        }

// <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Agro-Dev
        public async Task<bool> ExistsWithPropertyValue<T>(string namePropCheck, string valueCheck, string id = null) where T : DocumentBase
        {   
// ========================================================================
//         public async Task<bool> ExistsWithPropertyValue<T>(string namePropCheck, string valueCheck, string id, bool checkBatch) where T : DocumentBase {
//             var db = new MainGenericDb<T>(DbArguments);
//             long result = 0;
//             string query;
//             if (checkBatch) {
//                 query = $"SELECT value count(1) FROM c where c.Entity.{namePropCheck} = '{valueCheck}'";
//                 result += await db.BatchStore.QuerySingleAsync<long>(query);
//             }
//             query = $"SELECT value count(1) FROM c where c.{namePropCheck} = '{valueCheck}'";
// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Script_PoblarDB
            
            if (!string.IsNullOrWhiteSpace(id))
// <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Agro-Dev
                return await ExistsCustom<T>(Queries(DbQuery.COUNT_BY_NAMEVALUE_AND_NOID), namePropCheck, valueCheck,  id);

            return await ExistsCustom<T>(Queries(DbQuery.COUNT_BY_NAMEVALUE), namePropCheck, valueCheck);
        }

        public async Task<bool> ExistsCustom<T>(string query, params object[] args) where T : DocumentBase
        {   
            var result = await SingleQuery<T, long>(query, args);
// ========================================================================
//                 query += $" and c.id != '{id}'";
//             result += await db.Store.QuerySingleAsync<long>(query);
// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Script_PoblarDB
            return result != 0;
        }

        public async Task<bool> ExistsDosesFromOrder(string idDoses) => await ExistsCustom<ApplicationOrder>(Queries(DbQuery.COUNT_EXECUTION_OR_ORDERS_BY_DOSESID), idDoses);

        public async Task<bool> ExistsDosesExecutionOrder(string idDoses) => await ExistsCustom<ExecutionOrder>(Queries(DbQuery.COUNT_EXECUTION_OR_ORDERS_BY_DOSESID), idDoses);

    }

}