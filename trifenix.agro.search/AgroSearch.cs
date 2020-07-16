using Microsoft.Azure.Documents.Spatial;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.enums.query;
using trifenix.agro.enums.search;
using trifenix.agro.external.interfaces;
using trifenix.agro.search.interfaces;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.model;
using trifenix.connect.agro.model_input;
using trifenix.connect.mdm.az_search;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.indexes;
using trifenix.connect.mdm_attributes;
using trifenix.connect.util;

namespace trifenix.agro.search.operations
{
    /// <summary>
    /// Clase que interactua con azure search
    /// importante! es muy probable que los comentarios sean eliminados 
    /// refactorizar esta clase para soportar mejor el modelo.
    /// </summary>
    public class AgroSearch : IAgroSearch {
    
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
                CreateOrUpdateIndex<EntitySearch>(_entityIndex);
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
        /// <typeparam name="T">Tipo de entidad en el índicd</typeparam>
        /// <param name="indexName"></param>
        private void CreateOrUpdateIndex<T>(string indexName) {

            // dominios permitidos, cambiar para ponerlo en un json u otro archivo.
            string[] allowedOrigins = new string[] { "https://aresa.trifenix.io", "https://dev-aresa.trifenix.io", "https://agro.trifenix.io", "https://agro-dev.trifenix.io", "http://localhost:3000", "http://localhost:9009", "https://portal.azure.com" };

            // creación del índice.
            _search.Indexes.CreateOrUpdate(new Index { Name = indexName, Fields = FieldBuilder.BuildForType<T>(), CorsOptions = new CorsOptions(allowedOrigins) });
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
        public void AddElements(List<EntitySearch> elements) {
            OperationElements(elements, SearchOperation.Add);
        }

        /// <summary>
        /// Añade un elemento al search.
        /// </summary>
        /// <typeparam name="T">Esto debería ser EntitySearch</typeparam>
        /// <param name="elements"></param>
        public void AddElement(EntitySearch element)
        {
            OperationElements(new List<EntitySearch> { element}, SearchOperation.Add);
        }


        /// <summary>
        /// Borra elementos desde el search.
        /// </summary>        
        /// <param name="elements">entidades a eliminar</param>
        public void DeleteElements(List<EntitySearch> elements) {
            OperationElements(elements, SearchOperation.Delete);
        }

        /// <summary>
        /// filtra elementos del search de acuerdo a una conuslta
        /// </summary>
        /// <param name="filter">filtro de azure (Odata)</param>
        /// <returns>Entidades encontradas</returns>
        public List<EntitySearch> FilterElements(string filter) {
            var indexName = _entityIndex;
            var indexClient = _search.Indexes.GetClient(indexName);
            var result = indexClient.Documents.Search<EntitySearch>(null, new SearchParameters { Filter = filter });
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
        public EntitySearch[] GetElementsWithRelatedElement(EntityRelated elementToGet, EntityRelated relatedElement, string idRelatedElement) {
            var indexClient = _search.Indexes.GetClient(_entityIndex);
            var entities = indexClient.Documents.Search<EntitySearch>(null, new SearchParameters { Filter = string.Format(Queries(SearchQuery.ENTITIES_WITH_ENTITYID), (int)elementToGet, (int)relatedElement, idRelatedElement) }).Results.Select(s=>s.Document);
            return entities.ToArray();
        }


        /// <summary>
        /// Obtener una entidad, indicando el tipo y el identificador.
        /// </summary>
        /// <param name="entityRelated">Tipo entidad que obtendremos</param>
        /// <param name="id">identificador de la entidad</param>
        /// <returns></returns>
        public EntitySearch GetEntity(EntityRelated entityRelated, string id) {
            // cliente
            var indexClient = _search.Indexes.GetClient(_entityIndex);

            // consulta al search
            return indexClient.Documents.Search<EntitySearch>(null, new SearchParameters { Filter = string.Format(Queries(SearchQuery.GET_ELEMENT), (int)entityRelated, id) }).Results.FirstOrDefault()?.Document;
            
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
        /// Obtiene 
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public IEnumerable<RelatedId> GetArrayOfLocalRelatedIds(KeyValuePair<BaseIndexAttribute, object> attribute) {
            var typeValue = attribute.Value.GetType();
            if (typeValue == typeof(IEnumerable<string>))
                return ((IEnumerable<string>)attribute.Value).Select(s => new RelatedId { index = attribute.Key.Index, id = (string)s });
            else
                return new List<RelatedId>() { new RelatedId { index = attribute.Key.Index, id = (string)attribute.Value } };
        }

        public IEnumerable<RelatedId> GetArrayOfRelatedIds(KeyValuePair<BaseIndexAttribute, object> attribute) {
            if (attribute.Value.IsEnumerable()) {
                var relateds = new List<RelatedId>();
                foreach (var item in (IEnumerable<string>)attribute.Value)
                    relateds.Add(new RelatedId { index = attribute.Key.Index, id = item });
                return relateds;
            }
            else
                return new List<RelatedId>() { new RelatedId { index = attribute.Key.Index, id = (string)attribute.Value } };
        }
        
        private IEnumerable<BaseProperty<T>> GetArrayOfElements<T>(KeyValuePair<BaseIndexAttribute, object> attribute) {
            var typeValue = attribute.Value.GetType();
            if (attribute.Value.IsEnumerable())
                return ((IEnumerable<T>)attribute.Value).Select(s => GetProperty<T>(attribute.Key.Index, s));
            else
                return new List<BaseProperty<T>> { GetProperty<T>(attribute.Key.Index, attribute.Value)};
        }

        private IEnumerable<BaseProperty<T>> GetPropertiesObjects<T>(KindProperty related, Dictionary<BaseIndexAttribute, object> elements) =>
            elements.Where(s => !s.Key.IsEntity  && s.Key.KindIndex == (int)related).SelectMany(s => GetArrayOfElements<T>(s)).ToArray();

        private RelatedId[] GetReferences(Dictionary<BaseIndexAttribute, object> elements) =>
            elements.Where(s => s.Key.IsEntity).SelectMany(GetArrayOfRelatedIds).ToArray();

        
        private Num32Property[] GetNumProps(Dictionary<BaseIndexAttribute, object> values) =>
            GetPropertiesObjects<int>(KindProperty.NUM32, values).Select(s => new Num32Property { 
                index = s.index,
                value = s.value
            }).ToArray();

        private DblProperty[] GetDblProps(Dictionary<BaseIndexAttribute, object> values) =>
            GetPropertiesObjects<double>(KindProperty.DBL, values).Select(s => new DblProperty {
                index = s.index,
                value = s.value
            }).ToArray();

        private DtProperty[] GetDtProps(Dictionary<BaseIndexAttribute, object> values) =>
            GetPropertiesObjects<DateTime>(KindProperty.DATE, values).Select(s => new DtProperty {
                index = s.index,
                value = s.value
            }).ToArray();

        private EnumProperty[] GetEnumProps(Dictionary<BaseIndexAttribute, object> values) =>
            GetPropertiesObjects<int>(KindProperty.ENUM, values).Select(s => new EnumProperty {
                index = s.index,
                value = s.value
            }).ToArray();

        private BoolProperty[] GetBoolProps(Dictionary<BaseIndexAttribute, object> values) =>
            GetPropertiesObjects<bool>(KindProperty.BOOL, values).Select(s => new BoolProperty {
                index = s.index,
                value = s.value
            }).ToArray();

        private GeoProperty[] GetGeoProps(Dictionary<BaseIndexAttribute, object> values) =>
            GetPropertiesObjects<Point>(KindProperty.GEO, values).Select(s => new GeoProperty {
                index = s.index,
                value = GeographyPoint.Create(s.value.Position.Latitude, s.value.Position.Longitude)
            }).ToArray();

        private Num64Property[] GetNum64Props(Dictionary<BaseIndexAttribute, object> values) =>
            GetPropertiesObjects<long>(KindProperty.NUM64, values).Select(s => new Num64Property {
                index = s.index,
                value = s.value
            }).ToArray();

        private StrProperty[] GetStrProps(Dictionary<BaseIndexAttribute, object> values) =>
          GetPropertiesObjects<string>(KindProperty.STR, values).Select(s => new StrProperty {
              index = s.index,
              value = s.value
          }).ToArray();


        private EntitySearch GetEntitySearch(object obj, int index, string id) {

            // Código modificado para soportar que en vez de enviar un entity search, envie solo uno, dado un cambio en el requerimiento, donde se define un solo indice por entidad y no una colección.
            var entitySearch = new EntitySearch {
                id = id,
                index = index,
                created = DateTime.Now
            };
            var values = Mdm.Reflection.Attributes.GetPropertiesByAttributeWithValue(obj);
            if (!values.Any())
                return null;

            entitySearch.num32 = GetNumProps(values);
            entitySearch.dbl = GetDblProps(values);
            entitySearch.dt = GetDtProps(values);
            entitySearch.enm = GetEnumProps(values);
            entitySearch.bl = GetBoolProps(values);
            entitySearch.geo = GetGeoProps(values);
            entitySearch.num64 = GetNum64Props(values);
            entitySearch.str = GetStrProps(values);
            entitySearch.sug = GetStrProps(values);
            entitySearch.rel = GetReferences(values);
            var valuesWithoutProperty = Mdm.Reflection.Attributes.GetPropertiesWithoutAttributeWithValues(obj);

            foreach (var item in valuesWithoutProperty) {
                // porque llama a barrack solamente
                var value = GetEntitySearch(item, 0, string.Empty);
                entitySearch.num32 = entitySearch.num32.Union(value.num32).ToArray();
                entitySearch.dbl = entitySearch.dbl.Union(value.dbl).ToArray();
                entitySearch.dt = entitySearch.dt.Union(value.dt).ToArray();
                entitySearch.enm = entitySearch.enm.Union(value.enm).ToArray();
                entitySearch.geo = entitySearch.geo.Union(value.geo).ToArray();
                entitySearch.num64 = entitySearch.num64.Union(value.num64).ToArray();
                entitySearch.str = entitySearch.str.Union(value.str).ToArray();
                entitySearch.bl = entitySearch.bl.Union(value.bl).ToArray();
                entitySearch.sug = entitySearch.sug.Union(value.sug).ToArray();
                entitySearch.rel = entitySearch.rel.Union(value.rel).ToArray();
            }
            var localReference = values.Where(s => s.Key.IsEntity);
            if (localReference.Any()) {
                foreach (var item in localReference) {
                    IEnumerable<object> collection = item.Value.IsEnumerable() ? (IEnumerable<object>)item.Value : new List<object> { item.Value };
                    foreach (var childReferences in collection) {
                        var guid = Guid.NewGuid().ToString("N");
                        var localEntities = GetEntitySearch(childReferences, item.Key.Index, guid);
                        var listReferences = entitySearch.rel.ToList();
                        listReferences.Add(new RelatedId { id = guid, index = item.Key.Index });
                        entitySearch.rel = listReferences.ToArray();
                        return localEntities;
                    }
                }
            }
            
            return entitySearch;
        }
        public EntitySearch GetEntitySearch<T>(T entity)  where T : DocumentBase {
            var references = GetAttributes<EntityIndexAttribute>(typeof(T));
            if (references == null || !references.Any())
                return null;
            return GetEntitySearch(entity, references.First().Index, entity.Id);
        }

        public EntitySearch GetEntitySearchByInput<T>(T entity) where T : InputBase {
            var references = GetAttributes<EntityIndexAttribute>(typeof(T));
            if (references == null || !references.Any())
                return null;
            return GetEntitySearch(entity, references.First().Index, entity.Id);
        }

        

        


        



        

        public void AddDocument<T>(T document) where T : DocumentBase {
            AddElements(new List<EntitySearch> { GetEntitySearch(document) });
        }

        public void EmptyIndex<IndexSearch>(string indexName) {
            _search.Indexes.Delete(indexName);
            CreateOrUpdateIndex<IndexSearch>(indexName);
        }




        public async Task GenerateIndex(IAgroManager agro) {
            var assm = typeof(BusinessName).Assembly;
            var types = assm.GetTypes().Where(type => type.GetProperty("CosmosEntityName") != null && !(new[] { typeof(EntityContainer), typeof(User) }).Contains(type)).ToList();

            IEnumerable<Task> tasks;
            ConcurrentBag<object> bag;

            bag = new ConcurrentBag<object>();
            tasks = types.Select(async type =>
                bag.Add(GetElementsAndInsertIntoIndex(agro, type))
            );
            await Task.WhenAll(tasks);
        }

        private async Task GetElementsAndInsertIntoIndex(IAgroManager agro, Type dbType) {
            var extGetContainer = await agro.GetOperationByDbType(dbType).GetElements();
            var elements = extGetContainer.Result as List<DocumentBase>;
            elements?.ForEach(element => AddDocument(element));
        }

    }

}