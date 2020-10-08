using AutoMapper;
using Microsoft.Azure.Search.Models;
using System.Linq;
using trifenix.connect.agro_model_input;
using trifenix.connect.entities.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.interfaces.search;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.search.model;
using trifenix.connect.search;
using trifenix.connect.util;

namespace trifenix.connect.external
{
    public class EntitySearchMgmt<GeoPointType> : IEntitySearchOper<GeoPointType>
    {
        private MapperConfiguration mapper = new MapperConfiguration(cfg => cfg.CreateMap<EntitySearch, EntityBaseSearch<GeoPointType>>());

        private IBaseEntitySearch<GeoPointType> BaseSearch { get; }

        public EntitySearchMgmt(string SearchServiceName, string SearchServiceKey, string entityIndex, CorsOptions corsOptions) : this(new MainSearch<GeoPointType>(SearchServiceName, SearchServiceKey, entityIndex, corsOptions))
        {
        }

        public EntitySearchMgmt(IBaseEntitySearch<GeoPointType> bsearch)
        {
            BaseSearch = bsearch;
        }

        /// <summary>
        /// Toma un objeto cualquiera, lo convierte a un entitySearch y lo guarda en azure search.
        /// </summary>
        /// <typeparam name="T">tipo de dato tipo base de datos.</typeparam>
        /// <param name="document"></param>
        public void AddDocument<T>(T document) where T : DocumentBase
        {
            BaseSearch.AddElements(Mdm.GetEntitySearch(new Implements(), document, typeof(EntitySearch)).Cast<IEntitySearch<GeoPointType>>().ToList());
        }


        /// <summary>
        /// Obtiene un entitySearch desde un objeto de la base de datos
        /// el tipo de dato es por comodidad, si púede revisar internamente convierte cualquier objeto a entitySearch.
        /// Retorna una colección de EntitySearch, una de referencia y el resto local, ver atributos de la clase para más detalle.
        /// Vea EntityIndexAtribute, en el se asigna una referencia local o de referenci
        /// </summary>
        /// <see cref="EntityIndexAtribute"/>
        /// <typeparam name="T2">modelo del objeto que se convertirá a entity Search</typeparam>
        /// <param name="model">objeto a convertir</param>
        /// <returns>Colección de entity Search</returns>
        public IEntitySearch<GeoPointType>[] GetEntitySearch<T2>(T2 model) where T2 : DocumentBase
        {
            var mapperLocal = mapper.CreateMapper();
            return Mdm.GetEntitySearch(new Implements(), model, typeof(EntitySearch)).Select(mapperLocal.Map<EntityBaseSearch<GeoPointType>>).ToArray();
        }

        public IEntitySearch<GeoPointType>[] GetEntitySearchByInput<T2>(T2 model) where T2 : InputBase
        {
            var mapperLocal = mapper.CreateMapper();
            return Mdm.GetEntitySearch(new Implements(), model, typeof(EntitySearch)).Select(mapperLocal.Map<EntityBaseSearch<GeoPointType>>).ToArray();
        }
    }
}
