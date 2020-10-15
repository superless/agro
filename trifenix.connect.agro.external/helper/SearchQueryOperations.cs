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
    /// <summary>
    /// Especialización de IBaseEntitySearch
    /// donde usa los índices propios del proyecto agro.
    /// </summary>
    /// <typeparam name="GeoPointType"></typeparam>
    public class SearchQueryOperations<GeoPointType> : IRelatedSearch<GeoPointType>
    {

        // consultas 
        private readonly ISearchQueries _queries;

        

        private IBaseEntitySearch<GeoPointType> baseMainSearch;



        /// <summary>
        /// Consultas y mutación en Azure search.
        /// </summary>
        /// <param name="SearchServiceName">nombre del servicio</param>
        /// <param name="SearchServiceKey">clave del servicio</param>
        /// <param name="corsOptions">opciones de cors</param>
        public SearchQueryOperations(string SearchServiceName, string SearchServiceKey, string entityIndex, CorsOptions corsOptions) : this(new MainSearch<GeoPointType>(SearchServiceName, SearchServiceKey, entityIndex, corsOptions), new SearchQueries())
        {
        }


        /// <summary>
        /// IBaseEntitySearch que mantiene las operaciones hacia azure search, pero puede cambiar hacía otro motor.
        /// SearchQuery retorna consultas desde un diccionario
        /// </summary>
        /// <param name="baseMainSearch">IBaseEntitySearch con operaciones CRUD a un motor de base de datos o colección</param>
        /// <param name="searchQueries">Diccionario de consultas, dependiente del proyecto agro</param>
        public SearchQueryOperations(IBaseEntitySearch<GeoPointType> baseMainSearch, ISearchQueries searchQueries)
        {
            this.baseMainSearch = baseMainSearch;

            _queries = searchQueries;
        }

        /// <summary>
        /// Obtiene las consultas de Azure Search definidas
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
            var filter = string.Format(Queries(SearchQuery.ENTITIES_WITH_ENTITYID), (int)elementToGet, (int)relatedElement, idRelatedElement);

            return baseMainSearch.FilterElements(filter).ToArray();
        }


        /// <summary>
        /// Obtener una entidad, indicando el tipo y el identificador.
        /// </summary>
        /// <param name="entityRelated">Tipo entidad que obtendremos</param>
        /// <param name="id">identificador de la entidad</param>
        /// <returns></returns>
        public IEntitySearch<GeoPointType> GetEntity(EntityRelated entityRelated, string id)
        {
            var query = string.Format(Queries(SearchQuery.GET_ELEMENT), (int)entityRelated, id);
            // consulta al search

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
            var query = string.Format(Queries(SearchQuery.ENTITIES_WITH_ENTITYID), (int)elementToDelete, (int)relatedElement, idRelatedElement);
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
