using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;

namespace trifenix.agro.db.applicationsReference.agro.Common {
    public class Counters : BaseQueries, ICounters {

        public Counters(AgroDbArguments dbArguments) : base(dbArguments) { }

        public async Task<long> GetCounter<T>(string query) where T : DocumentBase => await Client<T>().QuerySingleAsync<long>(query);

        public async Task<long> GetLastCounterDoses(string idProduct) {
            var existAny = await SingleQuery<Dose, long>(Queries(DbQuery.COUNT_DOSES_BY_PRODUCTID), idProduct);
            if (existAny == 0)
                return 0;
            return await SingleQuery<Dose, long>(Queries(DbQuery.MAXCORRELATIVE_DOSES_BY_PRODUCTID), idProduct);
        }

        public async Task<long> GetCorrelativeFromDoses(string idDoses) => await SingleQuery<Dose, long>(Queries(DbQuery.CORRELATIVE_FROM_DOSESID), idDoses);

    }

}