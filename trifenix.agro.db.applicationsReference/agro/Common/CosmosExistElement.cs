using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.common;

namespace trifenix.agro.db.applicationsReference.agro.Common {
    public class CosmosExistElement : IExistElement {
        public AgroDbArguments DbArguments { get; }

        public CosmosExistElement(AgroDbArguments dbArguments) {
            DbArguments = dbArguments;
        }

        public async Task<bool> ExistsById<T>(string id) where T: DocumentBase
        {   
            var query = $"SELECT value count(1) FROM c where c.id = '{id}'";
            return await ExistsCustom<T>(query);
        }

        public async Task<bool> ExistsWithPropertyValue<T>(string namePropCheck, string valueCheck, string id = null) where T : DocumentBase
        {
           
            string query = $"SELECT value count(1) FROM c where c.{namePropCheck} = '{valueCheck}'";
            if (!string.IsNullOrWhiteSpace(id))
                query += $" and c.id != '{id}'";

            return await ExistsCustom<T>(query);
        }

        public async Task<bool> ExistsCustom<T>(string query) where T : DocumentBase
        {
            var db = new MainGenericDb<T>(DbArguments);
            var result = await db.Store.QuerySingleAsync<long>(query);
            return result != 0;
        }
    }
}