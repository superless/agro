using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro.Common
{
    public class CosmosExistElement : IExistElement {
        public AgroDbArguments DbArguments { get; }

        public CosmosExistElement(AgroDbArguments dbArguments) {
            DbArguments = dbArguments;
        }

        public async Task<bool> ExistsById<T>(string id, bool checkBatch) where T : DocumentBase {
            var db = new MainGenericDb<T>(DbArguments);
            long result = 0;
            string query;
            if (checkBatch) {
                query = $"SELECT value count(1) FROM c where c.Entity.Id = '{id}'";
                result += await db.BatchStore.QuerySingleAsync<long>(query);
            }
            query = $"SELECT value count(1) FROM c where c.id = '{id}'";
            result += await db.Store.QuerySingleAsync<long>(query);
            return result != 0;
        }

        public async Task<bool> ExistsWithPropertyValue<T>(string namePropCheck, string valueCheck, string id, bool checkBatch) where T : DocumentBase {
            var db = new MainGenericDb<T>(DbArguments);
            long result = 0;
            string query;
            if (checkBatch) {
                query = $"SELECT value count(1) FROM c where c.Entity.{namePropCheck} = '{valueCheck}'";
                result += await db.BatchStore.QuerySingleAsync<long>(query);
            }
            query = $"SELECT value count(1) FROM c where c.{namePropCheck} = '{valueCheck}'";
            if (!string.IsNullOrWhiteSpace(id))
                query += $" and c.id != '{id}'";
            result += await db.Store.QuerySingleAsync<long>(query);
            return result != 0;
        }

    }

}