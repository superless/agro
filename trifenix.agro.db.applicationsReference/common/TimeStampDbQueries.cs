using Cosmonaut;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using trifenix.agro.db.interfaces.common;

namespace trifenix.agro.db.applicationsReference.common {
    public class TimeStampDbQueries : ITimeStampDbQueries {
        
        protected readonly CosmosStoreSettings StoreSettings;

        public TimeStampDbQueries(AgroDbArguments args) {
            StoreSettings = new CosmosStoreSettings(args.NameDb, args.EndPointUrl, args.PrimaryKey);
        }
        
        public async Task<long[]> GetTimestamps<T>() where T : DocumentBase {
            var store = new CosmosStore<T>(StoreSettings);
            var result = await store.QueryMultipleAsync<long>("SELECT value c._ts FROM c");
            if (result == null)
                return new  List<long>().ToArray();
            return result.ToArray();
        }

    }
}
