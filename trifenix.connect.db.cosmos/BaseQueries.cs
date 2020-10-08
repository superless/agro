using Cosmonaut;
using System.Collections.Generic;
using System.Threading.Tasks;

using trifenix.connect.entities.cosmos;

namespace trifenix.connect.db.cosmos
{
    public class BaseQueries {

        public readonly CosmosDbArguments DbArguments;
        
        public BaseQueries(CosmosDbArguments dbArguments) {
            DbArguments = dbArguments;
            
        }

        public ICosmosStore<T> Client<T>() where T:DocumentBase => new MainGenericDb<T>(DbArguments).Store;

        public async Task<T> SingleQuery<TDOCUMENT,T>(string query, params object[] args) where TDOCUMENT : DocumentBase {
            var store = Client<TDOCUMENT>();
            var result = await store.QuerySingleAsync<T>(string.Format(query, args));
            return result;
        }

        public async Task<IEnumerable<T>> MultipleQuery<TDOCUMENT, T>(string query, params object[] args) where TDOCUMENT : DocumentBase {
            var store = Client<TDOCUMENT>();
            var result = await store.QueryMultipleAsync<T>(string.Format(query, args));
            return result;
        }

        

    }

}