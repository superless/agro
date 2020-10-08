using AutoMapper;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Linq;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.interfaces.search;
using trifenix.connect.agro.model_queries;
using trifenix.connect.agro.queries;
using trifenix.connect.interfaces.search;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.search.model;
using trifenix.connect.search;

namespace trifenix.connect.agro.external.helper
{
    public class SearchQueryOperations<GeoPointType> : IRelatedSearch<GeoPointType>
    {



        private readonly SearchServiceClient _search;

        // índice para las entidades, nombre del indice en azure
        private readonly string _entityIndex;

        // consultas 
        private readonly ISearchQueries _queries;

        private MapperConfiguration mapper = new MapperConfiguration(cfg => cfg.CreateMap<EntitySearch, EntityBaseSearch<GeoPointType>>());

        private IBaseEntitySearch<GeoPointType> baseMainSearch;



        /// <summary>
        /// Consultas y mutación en Azure search.
        /// </summary>
        /// <param name="SearchServiceName">nombre del servicio</param>
        /// <param name="SearchServiceKey">clave del servicio</param>
        /// <param name="corsOptions">opciones de cors</param>
        public SearchQueryOperations(string SearchServiceName, string SearchServiceKey, string entityIndex, CorsOptions corsOptions) : this(new MainSearch<GeoPointType>(SearchServiceName, SearchServiceKey, entityIndex, corsOptions))
        {

            // consultas genéricas de azure search.
            _queries = new SearchQueries();

            _entityIndex = entityIndex;

            // cliente de azure search.
            _search = new SearchServiceClient(SearchServiceName, new SearchCredentials(SearchServiceKey));




        }

        public SearchQueryOperations(IBaseEntitySearch<GeoPointType> baseMainSearch)
        {
            this.baseMainSearch = baseMainSearch;


            // crea índice de entidades si no existe.
            if (!_search.Indexes.Exists(_entityIndex))
                baseMainSearch.CreateOrUpdateIndex();
        }

        /// <summary>
        /// Obtiene las consultas de Azure Search definidasd
        /// </summary>
        /// <param name="query">Tipo de consulta</param>
        /// <returns>consulta</returns>
        private string Queries(SearchQuery query) => _queries.Get(query);


        /// <summary>
        /// Obtiene entidades de un tipo, que tengas un tipo asociado con identificador.
        /// por ejemplo, buscar todos los alumnos, que esten en la carrera de ingeniería informática.
        /// </summary>
        /// <param name="elementToGet">Elemento a obtener</param>
        /// <param name="relatedElement">elemento relacionado por el cual se debe filtrar</param>
        /// <param name="idRelatedElement">identificar del elemento relacionado</param>
        /// <returns></returns>
        public IEntitySearch<GeoPointType>[] GetElementsWithRelatedElement(EntityRelated elementToGet, EntityRelated relatedElement, string idRelatedElement)
        {
            var indexClient = _search.Indexes.GetClient(_entityIndex);
            var entities = indexClient.Documents.Search<EntitySearch>(null, new SearchParameters { Filter = string.Format(Queries(SearchQuery.ENTITIES_WITH_ENTITYID), (int)elementToGet, (int)relatedElement, idRelatedElement) }).Results.Select(s => s.Document);
            var mapperLocal = mapper.CreateMapper();
            return entities.Select(mapperLocal.Map<EntityBaseSearch<GeoPointType>>).ToArray();

        }


        /// <summary>
        /// Obtener una entidad, indicando el tipo y el identificador.
        /// </summary>
        /// <param name="entityRelated">Tipo entidad que obtendremos</param>
        /// <param name="id">identificador de la entidad</param>
        /// <returns></returns>
        public IEntitySearch<GeoPointType> GetEntity(EntityRelated entityRelated, string id)
        {
            // cliente
            var indexClient = _search.Indexes.GetClient(_entityIndex);


            var query = string.Format(Queries(SearchQuery.GET_ELEMENT), (int)entityRelated, id);
            // consulta al search

            var result = indexClient.Documents.Search<EntitySearch>(null, new SearchParameters { Filter = query }).Results.FirstOrDefault()?.Document;
            var mapperLocal = mapper.CreateMapper();


            return mapperLocal.Map<EntityBaseSearch<GeoPointType>>(result);

        }


        /// <summary>
        /// Elimina elementos que tengan una entidad asociada, por ejemplo, borraría todos los alumnos de ingeniería informática.
        /// </summary>
        /// <param name="elementToDelete">Entidad que será elimina</param>
        /// <param name="relatedElement">el elemento que debe estar presente para la consulta de elementos a eliminar</param>
        /// <param name="idRelatedElement">identificador de elemento relacionado que debe estar presenta para la consulta de elementos a eliminar</param>
        public void DeleteElementsWithRelatedElement(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement)
        {
            var query = string.Format(Queries(SearchQuery.ENTITIES_WITH_ENTITYID), (int)elementToDelete, (int)relatedElement, idRelatedElement);
            baseMainSearch.DeleteElements(query);
        }


        /// <summary>
        /// Elimina elementos que tengan una entidad asociada, por ejemplo, borraría todos los alumnos de ingeniería informática.
        /// a diferencia de DeleteElementsWithRelatedElement
        /// púede existir un elemento que no se borrará indicandolo con el campo elementExceptId.
        /// </summary>
        /// <param name="elementToDelete">Entidad que será elimina</param>
        /// <param name="relatedElement">el elemento que debe estar presente para la consulta de elementos a eliminar</param>
        /// <param name="idRelatedElement">identificador de elemento relacionado que debe estar presenta para la consulta de elementos a eliminar</param>
        /// <param name="elementExceptId"></param>
        public void DeleteElementsWithRelatedElementExceptId(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement, string elementExceptId)
        {
            // consulta para eliminar 
            var query = string.Format(Queries(SearchQuery.ENTITIES_WITH_ENTITYID_EXCEPTID), (int)elementToDelete, (int)relatedElement, idRelatedElement, elementExceptId);

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
            var query = string.Format(Queries(SearchQuery.GET_ELEMENT), (int)entityRelated, id);
            baseMainSearch.DeleteElements(query);
        }
    }
}
