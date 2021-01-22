
using Microsoft.Spatial;
using System;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.external;
using trifenix.connect.agro.external.hash;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.bus;
using trifenix.connect.entities.cosmos;
using trifenix.connect.input;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.search.model;
using trifenix.connect.search_mdl;
using trifenix.connect.util;

namespace trifenix.connect.app.cloud
{
    public static class Cloud
    {
        private static string connectionString = "Endpoint=sb://servicebus-trifenix.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=XylTzxpLEfnz3Aq0Rp6amObJC8Kkl98E3bcipBbMPsk=";


        //new MainSearch<GeographyPoint>("https://search-agro.search.windows.net", "7902C1E82BEEDC85AC0E535CF45DFC77", "entities-agro")
        public static AgroSearch<GeographyPoint> search = new AgroSearch<GeographyPoint>(@"https://search-agro.search.windows.net", "7902C1E82BEEDC85AC0E535CF45DFC77", null, new ImplementsSearch(), new HashEntityAgroSearch());



        public static string GetCosmosEntityName<T>() where T : DocumentBase
        {
            return ((T)Activator.CreateInstance(typeof(T))).CosmosEntityName;
        }

        public async static Task PushElement<T>(T element, string entityName) where T : InputBase
        {

            var bus = new ServiceBus(connectionString, "colageneration-servicebus");
            var opInstance = new OperationInstance<T>(element, element.Id, entityName, string.IsNullOrWhiteSpace(element.Id) ? "POST" : "PUT", string.Empty);

            await bus.PushElement(opInstance
            , entityName);
        }

        public static T[] GetElements<T>(EntityRelated entity) where T : DocumentBase
        {

            var entities = search.FilterElements($"index eq {(int)entity}");

            var sectors = entities.Select(s => (T)Mdm.GetEntityFromSearch(s, typeof(T), "trifenix.connect.agro_model", a => a, new SearchElement(), new HashEntityAgroSearch()));

            return sectors.ToArray();
        }

        public static T[] GetElements<T>(string filter) where T : DocumentBase
        {

            var entities = search.FilterElements(filter);

            var sectors = entities.Select(s => (T)Mdm.GetEntityFromSearch(s, typeof(T), "trifenix.connect.agro_model", a => a, new SearchElement(), new HashEntityAgroSearch()));

            return sectors.ToArray();
        }

        public static T GetElement<T>(EntityRelated entity, string id) where T : DocumentBase
        {

            var entities = search.FilterElements($"index eq {(int)entity} and id eq '{id}'");



            return (T)Mdm.GetEntityFromSearch(entities.FirstOrDefault(), typeof(T), "trifenix.connect.agro_model", a => a, new SearchElement(), new HashEntityAgroSearch());
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
