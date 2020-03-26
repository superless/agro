using Cosmonaut;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.applicationsReference.common;
using trifenix.agro.enums;
using trifenix.agro.enums.query;

namespace trifenix.agro.db.applicationsReference.agro.Common {
    public class BaseQueries {

        public readonly AgroDbArguments DbArguments;
        private readonly Queries _queries;
        public BaseQueries(AgroDbArguments dbArguments) {
            DbArguments = dbArguments;
            _queries = new Queries();
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

        public string Queries(DbQuery query) => _queries.Get(query);

    }

}