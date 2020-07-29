using Microsoft.Azure.Search.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Spatial;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.search.operations;
using trifenix.agro.servicebus.operations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.search_mdl;
using trifenix.connect.util;

namespace trifenix.connect.app.cloud
{
    public static class Cloud
    {
        private static string connectionString = "Endpoint=sb://agrobus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=28iKIG/njebR7TMqZh5e9KDdI0Jhv3D6gjUUI198Gog=";

        
        public static AgroSearch<GeographyPoint> search = new AgroSearch<GeographyPoint>("agrocloud", "6D2BA6AF3373D17A341F7A099BA8FA6A", null);

        public async static Task CreateSector(string name) {


            var bus = new ServiceBus(connectionString, "agroqueue");

            var sector = new SectorInput
            {
                Name = name
            };
            var opInstance = new OperationInstance<SectorInput>(sector, sector.Id, new Sector().CosmosEntityName, "POST", string.Empty);

            await bus.PushElement(opInstance
            , new Sector().CosmosEntityName);


        }

        public async static Task EditSector(string id, string name)
        {


            var bus = new ServiceBus(connectionString, "agroqueue");

            var sector = new SectorInput
            {
                Id = id,
                Name = name
            };
            var opInstance = new OperationInstance<SectorInput>(sector, sector.Id, new Sector().CosmosEntityName, "POST", string.Empty);

            await bus.PushElement(opInstance
            , new Sector().CosmosEntityName);


        }

        public static Sector[] GetSectors() {

            var entities = search.FilterElements($"index eq {(int)EntityRelated.SECTOR}");

            var sectors = entities.Select(s => (Sector)Mdm.GetEntityFromSearch(s, typeof(Sector), "trifenix.connect.agro_model", a=>a, new SearchElement()));

            return sectors.ToArray();
        }

        public static async Task<long> GetLastSectorCorrelative() {
            return 10;
        }


    }

    public class SearchElement : ISearchEntity<GeographyPoint>
    {
        public IEntitySearch<GeographyPoint> GetEntity(int entityKind, string idEntity)
        {
            return Cloud.search.GetEntity((EntityRelated)entityKind, idEntity);
        }
    }
}
