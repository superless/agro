using System.Threading.Tasks;
using trifenix.connect.agro.interfaces.db;
using trifenix.connect.agro.model_queries;
using trifenix.connect.agro_model;
using trifenix.connect.arguments;
using trifenix.connect.db.cosmos;
using trifenix.connect.model;

namespace trifenix.connect.agro.queries
{
    public class CosmosExistElement : BaseQueries, IDbExistsElements {
        
        public CosmosExistElement(CosmosDbArguments dbArguments): base(dbArguments) { }

        public string Queries(DbQuery query) => new Queries().Get(query);

        public async Task<bool> ExistsById<T>(string id) where T: DocumentDb =>
            await ExistsCustom<T>(Queries(DbQuery.COUNT_BY_ID),id);

        public async Task<bool> ExistsWithPropertyValue<T>(string namePropCheck, string valueCheck, string id = null) where T : DocumentDb
        {   
            if (!string.IsNullOrWhiteSpace(id))
                return await ExistsCustom<T>(Queries(DbQuery.COUNT_BY_NAMEVALUE_AND_NOID), namePropCheck, valueCheck,  id);
            return await ExistsCustom<T>(Queries(DbQuery.COUNT_BY_NAMEVALUE), namePropCheck, valueCheck);
        }

        public async Task<bool> ExistsCustom<T>(string query, params object[] args) where T : DocumentDb
        {   
            var result = await SingleQuery<T, long>(query, args);
            return result != 0;
        }

        /// <summary>
        /// Comprueba si existe una dosis en una orden de aplicacion
        /// </summary>
        /// <param name="idDoses"></param>
        /// <returns></returns>
        public async Task<bool> ExistsDosesFromOrder(string idDoses) =>
            await ExistsCustom<ApplicationOrder>(Queries(DbQuery.COUNT_EXECUTION_OR_ORDERS_BY_DOSESID), idDoses);

        /// <summary>
        /// Comprueba si existe una dosis en una orden de ejecución
        /// </summary>
        /// <param name="idDoses"></param>
        /// <returns></returns>
        public async Task<bool> ExistsDosesExecutionOrder(string idDoses) =>
            await ExistsCustom<ExecutionOrder>(Queries(DbQuery.COUNT_EXECUTION_OR_ORDERS_BY_DOSESID), idDoses);

    }

}