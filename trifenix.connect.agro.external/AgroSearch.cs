using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro.interfaces.search;
using trifenix.connect.agro.model_queries;
using trifenix.connect.agro.queries;
using trifenix.connect.interfaces.hash;
using trifenix.connect.interfaces.search;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.model;
using trifenix.connect.search;
using trifenix.connect.search_mdl;
using trifenix.connect.util;

namespace trifenix.connect.agro.external
{
    /// <summary>
    /// Clase que interactua con azure search
    /// Operaciones en Azure Search de acuerdo a la interfaz con las operaciones.
    /// </summary>
    public class AgroSearch<GeoPointType> : IAgroSearch<GeoPointType> 
    {

        public string UriService { get; private set; }

        public string ServiceKey { get; private set; }

        public string Index { get; private set; }

        public Dictionary<string, List<string>> Queried { get; set; } = new Dictionary<string, List<string>>();







        /// <summary>
        /// Consultas y mutación en Azure search.
        /// </summary>
        /// <param name="SearchServiceName">Nombre del servicio</param>
        /// <param name="SearchServiceKey">Clave del servicio</param>
        /// <param name="corsOptions">Opciones de cors</param>
        /// /// <param name="implements">Implement de search, para la generación de entitySearch</param>
        /// <param name="entityId">Índice del search</param>}
        /// <param name="hashOper">Convertidor de hasg</param>
        public AgroSearch(string SearchServiceName, string SearchServiceKey, Implements<GeoPointType> implements, IHashSearchHelper hashOper, ILogger logger, string entityId = "entities-agro") 
            : this(
                  (IBaseEntitySearch<GeoPointType>)new MainSearch(SearchServiceName, SearchServiceKey, entityId),
                  new SearchQueries(),
                  implements,
                  hashOper
        )
        {
            this.logger = logger;
        }


        public AgroSearch(IBaseEntitySearch<GeoPointType> mainSearch, ISearchQueries queries, Implements<GeoPointType> implements, IHashSearchHelper hashOper)
        {
            this.hashOper = hashOper;
            this.baseMainSearch = mainSearch;
            this.ServiceKey = mainSearch.ServiceKey;
            this.UriService = mainSearch.UriService;
            Index = mainSearch.Index;
            this.implements = implements;
            _queries = queries;


        }


        public void AddElement(IEntitySearch<GeoPointType> element)
        {
            AddToQueried(nameof(AgroSearch<GeoPointType>.AddElement), JsonConvert.SerializeObject(element));
            baseMainSearch.AddElement(element);
        }

        public void AddElements(List<IEntitySearch<GeoPointType>> elements)
        {
            AddToQueried(nameof(AgroSearch<GeoPointType>.AddElements), JsonConvert.SerializeObject(elements));
            baseMainSearch.AddElements(elements);
        }


        public void CreateOrUpdateIndex()
        {
            AddToQueried(nameof(AgroSearch<GeoPointType>.CreateOrUpdateIndex), "OK");
            baseMainSearch.CreateOrUpdateIndex();
        }

        public void DeleteElements(List<IEntitySearch<GeoPointType>> elements)
        {
            AddToQueried(nameof(AgroSearch<GeoPointType>.DeleteElements), JsonConvert.SerializeObject(elements));
            baseMainSearch.DeleteElements(elements);
        }

        public void DeleteElements(string query)
        {
            AddToQueried(nameof(AgroSearch<GeoPointType>.DeleteElements), query);
            baseMainSearch.DeleteElements(query);
        }

        

        public void EmptyIndex()
        {
            AddToQueried(nameof(AgroSearch<GeoPointType>.EmptyIndex), "OK");
            baseMainSearch.EmptyIndex();
        }

        public List<IEntitySearch<GeoPointType>> FilterElements(string filter)
        {
            AddToQueried(nameof(AgroSearch<GeoPointType>.FilterElements), filter);
            return baseMainSearch.FilterElements(filter);
        }

        // consultas 
        private readonly ISearchQueries _queries;

        // mapper que convierte un tipo que implemente un IEntitySearch y cree un EntityBaseSearch.
        private MapperConfiguration mapper = new MapperConfiguration(cfg => cfg.CreateMap<IEntitySearch<GeoPointType>, EntityBaseSearch<GeoPointType>>());

        // operaciones search de busquedas de trifenix connect.
        private IBaseEntitySearch<GeoPointType> baseMainSearch;

        // retorna los tipos de clase, de cada interface de un entitySearch.
        // con el objeto de crear un entitySearch y finalmente retornar un IEntitySearch<GeoPointType>
        readonly Implements<GeoPointType> implements;

        // operaciones hash para la cabecera y modelo de un entitySearch.
        readonly IHashSearchHelper hashOper;
        private readonly ILogger logger;







        /// <summary>
        /// Obtiene las consultas de Azure Search definidas
        /// </summary>
        /// <param name="query">Tipo de consulta</param>
        /// <returns>consulta</returns>
        private string Queries(SearchQuery query) => _queries.Get(query);


        /// <summary>
        /// Obtiene entidades de un tipo, que tengas un tipo asociado con identificador.
        /// por ejemplo, buscar todos los alumnos, que esten en la carrera de ingeniería informática.
        /// el related element sería la carrera y el elementToGet sería obtener el usuario.
        /// </summary>
        /// <param name="elementToGet">Elemento a obtener, por ejemplo el alumno</param>
        /// <param name="relatedElement">elemento relacionado por el cual se debe filtrar, por ejemplo la carrera</param>
        /// <param name="idRelatedElement">identificar del elemento relacionado</param>
        /// <returns>Listado de entitySearch</returns>
        public IEntitySearch<GeoPointType>[] GetElementsWithRelatedElement(EntityRelated elementToGet, EntityRelated relatedElement, string idRelatedElement)
        {
            // format query
            var filter = string.Format(Queries(SearchQuery.ENTITIES_WITH_ENTITYID), (int)elementToGet, (int)relatedElement, idRelatedElement);

            // añade a consultas
            AddToQueried(nameof(AgroSearch<GeoPointType>.GetElementsWithRelatedElement), filter);

            // retorna elementos de acuerdo al filtro.
            return baseMainSearch.FilterElements(filter).ToArray();
        }

        /// <summary>
        /// añade a la cola de consultas
        /// </summary>
        /// <param name="methodName">nombre del método</param>
        /// <param name="query">consulta utilizada, puede ser un json</param>
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
        /// Toma un objeto de base de datos de persistencia, lo convierte a un entitySearch y lo guarda en azure search.
        /// </summary>
        /// <typeparam name="T">tipo de dato tipo base de datos.</typeparam>
        /// <param name="document">entidad o documento de base de datos de persistencia</param>
        public void AddDocument<T>(T document) where T : DocumentDb
        {

            var dateCreateDocument = DateTime.Now;

            logger?.LogInformation($"[{dateCreateDocument:s}] creando elemento entitySearch de {document.DocumentPartition}");
            // obtiene un entitySearch desde una entidad de base de datos de persistencia.
            var entitySearch = Mdm.Reflection.Entities.GetEntitySearch(implements, document, hashOper).Cast<IEntitySearch<GeoPointType>>().ToList();

            var dateCreatedDocument = DateTime.Now;
            logger?.LogInformation($"[{dateCreatedDocument:s}] {document.DocumentPartition} es conviertido a entitySearch en {(dateCreatedDocument - dateCreateDocument).TotalSeconds} segundos");
            // añade a registro
            AddToQueried(nameof(AgroSearch<GeoPointType>.AddDocument), JsonConvert.SerializeObject(entitySearch));


            var dateCreateSearch = DateTime.Now;

            logger?.LogInformation($"[{dateCreateSearch:s}] Guardando entitySearch de  {document.DocumentPartition}");
            // añade a base de datos.
            baseMainSearch.AddElements(entitySearch);

            var dateCreatedSearch = DateTime.Now;

            logger?.LogInformation($"[{dateCreatedSearch:s}] entitySearch de  {document.DocumentPartition} guardado en search en {(dateCreatedDocument - dateCreateDocument).TotalSeconds} segundps");
        }


        /// <summary>
        /// Obtener una entidad, indicando el tipo y el identificador.
        /// </summary>
        /// <param name="entityRelated">Tipo entidad que obtendremos</param>
        /// <param name="id">identificador de la entidad</param>
        /// <returns>EntitySearch del tipo solicitado</returns>
        public IEntitySearch<GeoPointType> GetEntity(EntityRelated entityRelated, string id)
        {
            // obtiene la consulta desde un diccionario y asigna los parámetros
            var query = string.Format(Queries(SearchQuery.GET_ELEMENT), (int)entityRelated, id);
            // consulta al search
            AddToQueried(nameof(AgroSearch<GeoPointType>.GetEntity), query);

            // primer elemento de la consulta.
            return baseMainSearch.FilterElements(query)?.FirstOrDefault();
        }

        /// <summary>
        /// Elimina elementos que tengan una entidad asociada, por ejemplo, borraría todos los alumnos de ingeniería informática.
        /// </summary>
        /// <param name="elementToDelete">Entidad que será elimina</param>
        /// <param name="relatedElement">el elemento que debe estar presente para la consulta de elementos a eliminar</param>
        /// <param name="idRelatedElement">identificador de elemento relacionado que debe estar presenta para la consulta de elementos a eliminar</param>
        public void DeleteElementsWithRelatedElement(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement)
        {
            // obtiene la consulta desde un diccionario y asigna los parámetros
            var query = string.Format(Queries(SearchQuery.ENTITIES_WITH_ENTITYID), (int)elementToDelete, (int)relatedElement, idRelatedElement);

            // añade al registro
            AddToQueried(nameof(AgroSearch<GeoPointType>.DeleteElementsWithRelatedElement), query);

            // elimina los elementos encontrados en la consulta.
            baseMainSearch.DeleteElements(query);
        }


        /// <summary>
        /// Elimina elementos que tengan una entidad asociada, por ejemplo, borraría todos los alumnos de ingeniería informática, pero no los de arquitectura.
        /// a diferencia de DeleteElementsWithRelatedElement
        /// púede existir un elemento que no se borrará indicandolo con el campo elementExceptId.
        /// borraría todos los alumnos de informática menos el id del alumno ingresado
        /// </summary>
        /// <param name="elementToDelete">Entidad que será eliminada, por ejemplo alumnos</param>
        /// <param name="relatedElement">Entidad que contiene la agrupación, por ejemplo carrera</param>
        /// <param name="idRelatedElement">identificador de la entidad que contiene la colección, por ejemplo carrera = informatica</param>
        /// <param name="elementExceptId">identificador de elemento que no debe ser eliminado dentro del grupo, por ejemplo, el alumno juan garcia</param>
        public void DeleteElementsWithRelatedElementExceptId(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement, string elementExceptId)
        {
            // consulta para eliminar 
            var query = string.Format(Queries(SearchQuery.ENTITIES_WITH_ENTITYID_EXCEPTID), (int)elementToDelete, (int)relatedElement, idRelatedElement, elementExceptId);

            //añade al registro.
            AddToQueried(nameof(AgroSearch<GeoPointType>.DeleteElementsWithRelatedElementExceptId), query);
            // eliminación.
            baseMainSearch.DeleteElements(query);
        }


        /// <summary>
        /// Elimina entidades que tengan un tipo y un id.
        /// </summary>
        /// <param name="entityRelated">Tipo de elemento a eliminar</param>
        /// <param name="id">identificador de la entidad</param>
        public void DeleteEntity(EntityRelated entityRelated, string id)
        {
            // consulta desde diccionario
            var query = string.Format(Queries(SearchQuery.GET_ELEMENT), (int)entityRelated, id);

            // registro de consulta
            AddToQueried(nameof(AgroSearch<GeoPointType>.DeleteEntity), query);

            // elimina elementos de la consulta.
            baseMainSearch.DeleteElements(query);
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
        public IEntitySearch<GeoPointType>[] GetEntitySearch<T2>(T2 model) where T2 : DocumentDb
        {
            // crea un mapper para convertir un IEntitySearch a EntityBaseSearch<GeoPointType>
            var mapperLocal = mapper.CreateMapper();

            // convierte un elemento de persistencia a un entitySearch<GeoPointType>
            var document = Mdm.Reflection.Entities.GetEntitySearch(
                                implements, // asignación de tipos para cada interface de un entitySearch
                                model // documento a convertir                                
                                , hashOper)
                                .Select(mapperLocal.Map<EntityBaseSearch<GeoPointType>>) // mapea el entitySearch de tipo GetEntitySearchImplementedType y lo convierte en EntityBaseSearch<GeoPointType>.
                                .ToArray();


            return document;
        }





















    }

}