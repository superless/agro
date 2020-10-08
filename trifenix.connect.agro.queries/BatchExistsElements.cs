using System.Threading.Tasks;
using trifenix.connect.agro.model_queries;
using trifenix.connect.db;
using trifenix.connect.db.cosmos;
using trifenix.connect.entities.cosmos;
using trifenix.connect.interfaces.db.cosmos;

namespace trifenix.connect.agro.queries
{

    public class BatchExistsElements : BaseQueries, IExistElement {

        public string Queries(DbQuery query) => new Queries().Get(query);

        public BatchExistsElements(CosmosDbArguments dbArguments) : base(dbArguments) { }

        public async Task<bool> ExistsById<T>(string id) where T : DocumentBase =>
            await ExistsCustom<T>(Queries(DbQuery.COUNT_BY_ID), id) || await ExistsCustom<EntityContainer>("SELECT value count(1) FROM c where c.Entity.Id = '{0}'", id);

        public async Task<bool> ExistsWithPropertyValue<T>(string namePropCheck, string valueCheck, string id = null) where T : DocumentBase => !string.IsNullOrWhiteSpace(id) ? 
            await ExistsCustom<T>(Queries(DbQuery.COUNT_BY_NAMEVALUE_AND_NOID), namePropCheck, valueCheck, id) : await ExistsCustom<T>(Queries(DbQuery.COUNT_BY_NAMEVALUE), namePropCheck, valueCheck);

        private async Task<bool> ExistsCustom<T>(string query, params object[] args) where T : DocumentBase {   
            var result = await SingleQuery<T, long>(query, args);
            return result != 0;
        }


    }

}