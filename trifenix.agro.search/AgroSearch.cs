using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.enums.query;
using trifenix.agro.enums.search;
using trifenix.agro.external.interfaces;
using trifenix.agro.search.interfaces;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.mdm.az_search;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.util;

namespace trifenix.agro.search.operations {
    /// <summary>
    /// Clase que interactua con azure search
    /// importante! es muy probable que los comentarios sean eliminados 
    /// refactorizar esta clase para soportar mejor el modelo.
    /// </summary>
    public class AgroSearch<GeoPointType> : IAgroSearch<GeoPointType> {
    
        // cliente azure
        private readonly SearchServiceClient _search;

        // índice para las entidades, nombre del indice en azure
        private readonly string _entityIndex = "entities";

       
        // consultas 
        private readonly ISearchQueries _queries;

       
        /// <summary>
        /// Consultas y mutación en Azure search.
        /// </summary>
        /// <param name="SearchServiceName">nombre del servicio</param>
        /// <param name="SearchServiceKey">clave del servicio</param>
        public AgroSearch(string SearchServiceName, string SearchServiceKey) {

            // consultas genéricas de azure search.
            _queries = new SearchQueries();

            // cliente de azure search.
            _search = new SearchServiceClient(SearchServiceName, new SearchCredentials(SearchServiceKey));


            // crea índice de entidades si no existe.
            if (!_search.Indexes.Exists(_entityIndex))
                CreateOrUpdateIndex(_entityIndex);
        }

       /// <summary>
       /// Obtiene las consultas de Azure Search definidasd
       /// </summary>
       /// <param name="query">Tipo de consulta</param>
       /// <returns>consulta</returns>
        private string Queries(SearchQuery query) => _queries.Get(query);

        
        /// <summary>
        /// Crea índice en el azure search.
        /// </summary>        
        /// <param name="indexName"></param>
        private void CreateOrUpdateIndex(string indexName) {

            // dominios permitidos, cambiar para ponerlo en un json u otro archivo.
            string[] allowedOrigins = new string[] { "https://aresa.trifenix.io", "https://dev-aresa.trifenix.io", "https://agro.trifenix.io", "https://agro-dev.trifenix.io", "http://localhost:3000", "http://localhost:9009", "https://portal.azure.com" };

            // creación del índice.
            _search.Indexes.CreateOrUpdate(new Index { Name = indexName, Fields = FieldBuilder.BuildForType<EntitySearch>(), CorsOptions = new CorsOptions(allowedOrigins) });
        }

        /// <summary>
        /// Añade o elimina items dentro de azure search.
        /// </summary>
        /// <typeparam name="T">El tipo solo puede ser una entidad soportada dentro de azure search, se validará que cumpla</typeparam>
        /// <param name="elements">elementos a guardar dentro del search</param>
        /// <param name="operationType">Tipo de operación Añadir o borrar</param>
        private void OperationElements<T>(List<T> elements, SearchOperation operationType) {

            // validar que sea un elemento de tipo search.
            var indexName = _entityIndex;

            // obtiene el client azure search de acuerdo al índice.
            var indexClient = _search.Indexes.GetClient(indexName);

            // realiza la acción segun el argumento
            var actions = elements.Select(o => operationType == SearchOperation.Add ? IndexAction.Upload(o) : IndexAction.Delete(o));

            // preparando la ejecución
            var batch = IndexBatch.New(actions);

            // ejecución.
            indexClient.Documents.Index(batch);
        }

        /// <summary>
        /// Añade elementos al search.
        /// </summary>
        /// <typeparam name="T">Esto debería ser EntitySearch</typeparam>
        /// <param name="elements"></param>
        public void AddElements(List<IEntitySearch<GeoPointType>> elements) {
            OperationElements(elements, SearchOperation.Add);
        }

        /// <summary>
        /// Añade un elemento al search.
        /// </summary>
        /// <typeparam name="T">Esto debería ser EntitySearch</typeparam>
        /// <param name="elements"></param>
        public void AddElement(IEntitySearch<GeoPointType> element)
        {
            OperationElements(new List<IEntitySearch<GeoPointType>> { element}, SearchOperation.Add);
        }

        //public async Task DeleteElements(IAgroManager agro, Type dbType) {
        //    var extGetContainer = await agro.GetOperationByDbType(dbType).GetElements();
        //    var elements = extGetContainer.Result as List<DocumentBase>;
        //    DeleteElements(elements.SelectMany(element => GetEntitySearch(element)).ToList());
        //}

        /// <summary>
        /// Borra elementos desde el search.
        /// </summary>        
        /// <param name="elements">entidades a eliminar</param>
        public void DeleteElements(List<IEntitySearch<GeoPointType>> elements) {
            OperationElements(elements, SearchOperation.Delete);
        }

        /// <summary>
        /// filtra elementos del search de acuerdo a una conuslta
        /// </summary>
        /// <param name="filter">filtro de azure (Odata)</param>
        /// <returns>Entidades encontradas</returns>
        public List<IEntitySearch<GeoPointType>> FilterElements(string filter) {
            var indexName = _entityIndex;
            var indexClient = _search.Indexes.GetClient(indexName);
            var result = indexClient.Documents.Search<IEntitySearch<GeoPointType>>(null, new SearchParameters { Filter = filter });
            return result.Results.Select(v => v.Document).ToList();
        }


        /// <summary>
        /// Borrar elementos de azure search de acuerdo auna consulta
        /// </summary>        
        /// <param name="query">consulta de elementos a eliminar</param>
        public void DeleteElements(string query) {
            var elements = FilterElements(query);
            if (elements.Any())
                DeleteElements(elements);
        }


        /// <summary>
        /// Obtiene entidades de un tipo, que tengas un tipo asociado con identificador.
        /// por ejemplo, buscar todos los alumnos, que esten en la carrera de ingeniería informática.
        /// </summary>
        /// <param name="elementToGet">Elemento a obtener</param>
        /// <param name="relatedElement">elemento relacionado por el cual se debe filtrar</param>
        /// <param name="idRelatedElement">identificar del elemento relacionado</param>
        /// <returns></returns>
        public IEntitySearch<GeoPointType>[] GetElementsWithRelatedElement(EntityRelated elementToGet, EntityRelated relatedElement, string idRelatedElement) {
            var indexClient = _search.Indexes.GetClient(_entityIndex);
            var entities = indexClient.Documents.Search<IEntitySearch<GeoPointType>>(null, new SearchParameters { Filter = string.Format(Queries(SearchQuery.ENTITIES_WITH_ENTITYID), (int)elementToGet, (int)relatedElement, idRelatedElement) }).Results.Select(s=>s.Document);
            return entities.ToArray();
        }


        /// <summary>
        /// Obtener una entidad, indicando el tipo y el identificador.
        /// </summary>
        /// <param name="entityRelated">Tipo entidad que obtendremos</param>
        /// <param name="id">identificador de la entidad</param>
        /// <returns></returns>
        public IEntitySearch<GeoPointType> GetEntity(EntityRelated entityRelated, string id) {
            // cliente
            var indexClient = _search.Indexes.GetClient(_entityIndex);

            // consulta al search
            return indexClient.Documents.Search<IEntitySearch<GeoPointType>>(null, new SearchParameters { Filter = string.Format(Queries(SearchQuery.GET_ELEMENT), (int)entityRelated, id) }).Results.FirstOrDefault()?.Document;
            
        }


        /// <summary>
        /// Elimina elementos que tengan una entidad asociada, por ejemplo, borraría todos los alumnos de ingeniería informática.
        /// </summary>
        /// <param name="elementToDelete">Entidad que será elimina</param>
        /// <param name="relatedElement">el elemento que debe estar presente para la consulta de elementos a eliminar</param>
        /// <param name="idRelatedElement">identificador de elemento relacionado que debe estar presenta para la consulta de elementos a eliminar</param>
        public void DeleteElementsWithRelatedElement(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement) {
            var query = string.Format(Queries(SearchQuery.ENTITIES_WITH_ENTITYID), (int)elementToDelete, (int)relatedElement, idRelatedElement);
            DeleteElements(query);
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
        public void DeleteElementsWithRelatedElementExceptId(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement, string elementExceptId) {
            // consulta para eliminar 
            var query = string.Format(Queries(SearchQuery.ENTITIES_WITH_ENTITYID_EXCEPTID), (int)elementToDelete, (int)relatedElement, idRelatedElement, elementExceptId);

            // eliminación.
            DeleteElements(query);
        }


        /// <summary>
        /// Elimina entridades que tengan un tipo y un id.
        /// </summary>
        /// <param name="entityRelated">Tipo de elemento a eliminar</param>
        /// <param name="id">identificador de la entidad</param>
        public void DeleteEntity(EntityRelated entityRelated, string id) {
            var query = string.Format(Queries(SearchQuery.GET_ELEMENT), (int)entityRelated, id);
            DeleteElements(query);
        }

        /// <summary>
        /// Toma un objeto cualquiera, lo convierte a un entitySearch y lo guarda en azure search.
        /// </summary>
        /// <typeparam name="T">tipo de dato tipo base de datos.</typeparam>
        /// <param name="document"></param>
        public void AddDocument<T>(T document) where T : DocumentBase {
            AddElements(Mdm.GetEntitySearch(new Implements(), document, typeof(EntitySearch)).Cast<IEntitySearch<GeoPointType>>().ToList());
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
            return (IEntitySearch<GeoPointType>[])Mdm.GetEntitySearch(new Implements(), model, typeof(EntitySearch)).Cast<EntitySearch>().ToArray();
        }

        public IEntitySearch<GeoPointType>[] GetEntitySearchByInput<T2>(T2 model) where T2: InputBase
        {
            return (IEntitySearch<GeoPointType>[])Mdm.GetEntitySearch(new Implements(), model, typeof(EntitySearch)).Cast<EntitySearch>().ToArray();
        }





        public void EmptyIndex(string indexName) {
            _search.Indexes.Delete(indexName);
            CreateOrUpdateIndex(indexName);
        }




        public async Task GenerateIndex(IAgroManager<GeoPointType> agro) {
            var assm = typeof(BusinessName).Assembly;
            var types = assm.GetTypes().Where(type => type.GetProperty("CosmosEntityName") != null && !(new[] { typeof(EntityContainer), typeof(User), typeof(UserActivity) }).Contains(type)).ToList();

            foreach (var type in types) {
                try {
                    await GetElementsAndInsertIntoIndex(agro, type);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        private async Task GetElementsAndInsertIntoIndex(IAgroManager<GeoPointType> agro, Type dbType) {
            var extGetContainer = await agro.GetOperationByDbType(dbType).GetElements();
            var elements = (IEnumerable<dynamic>)extGetContainer.Result;
            elements?.ToList().ForEach(element => AddDocument(element));
        }

       
    }

}