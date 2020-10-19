using AutoMapper;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using trifenix.connect.agro_model_input;
using trifenix.connect.entities.cosmos;
using trifenix.connect.input;
using trifenix.connect.interfaces.external;
using trifenix.connect.interfaces.search;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.search.model;
using trifenix.connect.search;
using trifenix.connect.search_mdl;
using trifenix.connect.util;

namespace trifenix.connect.agro.external
{
    public class EntitySearchMgmt<GeoPointType> : IEntitySearchOper<GeoPointType>
    {
        private MapperConfiguration mapper = new MapperConfiguration(cfg => cfg.CreateMap<EntitySearch, EntityBaseSearch<GeoPointType>>());
        readonly Implements<GeoPointType> implements;

        private IBaseEntitySearch<GeoPointType> BaseSearch { get; }

        public Dictionary<string, List<string>> Queried { get; set; } = new Dictionary<string, List<string>>();

        public EntitySearchMgmt(string SearchServiceName, string SearchServiceKey, string entityIndex, CorsOptions corsOptions, Implements<GeoPointType> implements) : this(new MainSearch<GeoPointType>(SearchServiceName, SearchServiceKey, entityIndex, corsOptions), implements)
        {
        }

        public EntitySearchMgmt(IBaseEntitySearch<GeoPointType> bsearch, Implements<GeoPointType> implements)
        {
            this.implements = implements;
            BaseSearch = bsearch;
        }

        /// <summary>
        /// Toma un objeto cualquiera, lo convierte a un entitySearch y lo guarda en azure search.
        /// </summary>
        /// <typeparam name="T">tipo de dato tipo base de datos.</typeparam>
        /// <param name="document"></param>
        public void AddDocument<T>(T document) where T : DocumentBase
        {
            var entitySearch = Mdm.GetEntitySearch(implements, document, typeof(EntitySearch)).Cast<IEntitySearch<GeoPointType>>().ToList();

            AddToQueried(nameof(EntitySearchMgmt<GeoPointType>.AddDocument), JsonConvert.SerializeObject(entitySearch));
            BaseSearch.AddElements(entitySearch);
        }

        private void AddToQueried(string methodName, string query)
        {
            if (!Queried.ContainsKey(methodName))
            {
                Queried.Add(methodName, new List<string> { query });
            }
            else
            {
                Queried[methodName].Add(query);
            }
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
            var document = Mdm.GetEntitySearch(implements, model, typeof(EntitySearch)).Select(mapperLocal.Map<EntityBaseSearch<GeoPointType>>).ToArray();
            
            return document;
        }

        public IEntitySearch<GeoPointType>[] GetEntitySearchByInput<T2>(T2 model) where T2 : InputBase
        {
            var mapperLocal = mapper.CreateMapper();
            var documents = Mdm.GetEntitySearch(implements, model, typeof(EntitySearch));
            var document = documents.Select(mapperLocal.Map<EntityBaseSearch<GeoPointType>>).ToArray();
            
            return document;
        }
    }
}
