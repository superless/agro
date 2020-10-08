using Cosmonaut;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.entities.cosmos;

namespace trifenix.connect.db.cosmos
{
    public class TimeStampDbQueries : ITimeStampDbQueries {
        
        protected readonly CosmosStoreSettings StoreSettings;

        public TimeStampDbQueries(CosmosDbArguments args) {
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
