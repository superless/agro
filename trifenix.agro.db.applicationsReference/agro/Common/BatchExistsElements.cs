using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model;
using trifenix.agro.enums;
using trifenix.agro.enums.query;
using trifenix.connect.agro_model;

namespace trifenix.agro.db.applicationsReference.agro.Common {

    public class BatchExistsElements : BaseQueries, IExistElement {

        public BatchExistsElements(AgroDbArguments dbArguments) : base(dbArguments) { }

        public async Task<bool> ExistsById<T>(string id) where T : DocumentBase =>
            await ExistsCustom<T>(Queries(DbQuery.COUNT_BY_ID), id) || await ExistsCustom<EntityContainer>("SELECT value count(1) FROM c where c.Entity.Id = '{0}'", id);

        public async Task<bool> ExistsWithPropertyValue<T>(string namePropCheck, string valueCheck, string id = null) where T : DocumentBase => !string.IsNullOrWhiteSpace(id) ? 
            await ExistsCustom<T>(Queries(DbQuery.COUNT_BY_NAMEVALUE_AND_NOID), namePropCheck, valueCheck, id) : await ExistsCustom<T>(Queries(DbQuery.COUNT_BY_NAMEVALUE), namePropCheck, valueCheck);

        private async Task<bool> ExistsCustom<T>(string query, params object[] args) where T : DocumentBase {   
            var result = await SingleQuery<T, long>(query, args);
            return result != 0;
        }

        public Task<bool> ExistsDosesFromOrder(string idDoses) {
            throw new System.NotImplementedException();
        }

        public Task<bool> ExistsDosesExecutionOrder(string idDoses) {
            throw new System.NotImplementedException();
        }

    }

}