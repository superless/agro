using Microsoft.Azure.Documents.Spatial;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using trifenix.agro.attr;
using trifenix.agro.db;
using trifenix.agro.enums.query;
using trifenix.agro.enums.search;
using trifenix.agro.enums.searchModel;
using trifenix.agro.model.external.Input;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.search.operations.util;
using V2 = trifenix.agro.search.model.temp;
using static trifenix.agro.search.operations.util.AgroHelper;
using Newtonsoft.Json;
using System.Collections;

namespace trifenix.agro.search.operations {

    public class AgroSearch : IAgroSearch {
    
        private readonly SearchServiceClient _search;
        private readonly string _entityIndex = "entities-v2";
        private readonly string _commentIndex = "comments";

        private readonly ISearchQueries _queries;

        // para activar auto-complete, no es activado aún debido a el tamaño que toma.
        //private readonly List<Suggester> _suggesterEntities = new List<Suggester> {
        //    new Suggester {
        //        Name= "SgProperty",
        //        SourceFields = new List<string>{ 
        //            "RelatedProperties/Value"
        //        }
        //    }
        //};

        public AgroSearch(string SearchServiceName, string SearchServiceKey) {
            _queries = new SearchQueries();
            _search = new SearchServiceClient(SearchServiceName, new SearchCredentials(SearchServiceKey));
            if (!_search.Indexes.Exists(_entityIndex))
                _search.Indexes.CreateOrUpdate(new Index { Name = _entityIndex, Fields = FieldBuilder.BuildForType<V2.EntitySearchV2>() });
            if (!_search.Indexes.Exists(_commentIndex))
                _search.Indexes.CreateOrUpdate(new Index { Name = _commentIndex, Fields = FieldBuilder.BuildForType<CommentSearch>() });
        }

        private string Queries(SearchQuery query) => _queries.Get(query);

        private void OperationElements<T>(List<T> elements, SearchOperation operationType) {
            var indexName = typeof(T).Equals(typeof(V2.EntitySearchV2)) ? _entityIndex : _commentIndex;
            var indexClient = _search.Indexes.GetClient(indexName);
            var actions = elements.Select(o => operationType == SearchOperation.Add ? IndexAction.Upload(o) : IndexAction.Delete(o));
            var batch = IndexBatch.New(actions);
            indexClient.Documents.Index(batch);
        }

        public void AddElements<T>(List<T> elements) {
            OperationElements(elements, SearchOperation.Add);
        }

        public void DeleteElements<T>(List<T> elements) {
            OperationElements(elements, SearchOperation.Delete);
        }

        public List<T> FilterElements<T>(string filter) {
            var indexName = typeof(T).Equals(typeof(EntitySearch)) ? _entityIndex : _commentIndex;
            var indexClient = _search.Indexes.GetClient(indexName);
            var result = indexClient.Documents.Search<T>(null, new SearchParameters { Filter = filter });
            return result.Results.Select(v => v.Document).ToList();
        }

        public void DeleteElements<T>(string query) {
            var elements = FilterElements<EntitySearch>(query);
            if (elements.Any())
                DeleteElements(elements);
        }

        public EntitySearch GetEntity(EntityRelated entityRelated, string id) {
            var indexClient = _search.Indexes.GetClient(_entityIndex);
            var entity = indexClient.Documents.Search<EntitySearch>(null, new SearchParameters { Filter = string.Format(Queries(SearchQuery.GET_ELEMENT), (int)entityRelated, id) }).Results.FirstOrDefault()?.Document;
            return entity;
        }

        public V2.EntitySearchV2 GetEntityV2(EntityRelated entityRelated, string id) {
            var indexClient = _search.Indexes.GetClient(_entityIndex);
            var entity = indexClient.Documents.Search<V2.EntitySearchV2>(null, new SearchParameters { Filter = string.Format(Queries(SearchQuery.GET_ELEMENT), (int)entityRelated, id) }).Results.FirstOrDefault()?.Document;
            return entity;
        }

        public void DeleteElementsWithRelatedElement(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement) {
            var query = string.Format(Queries(SearchQuery.ENTITIES_WITH_ENTITYID), (int)elementToDelete, (int)relatedElement, idRelatedElement);
            DeleteElements<EntitySearch>(query);
        }

        public void DeleteElementsWithRelatedElementExceptId(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement, string elementExceptId) {
            var query = string.Format(Queries(SearchQuery.ENTITIES_WITH_ENTITYID_EXCEPTID), (int)elementToDelete, (int)relatedElement, idRelatedElement, elementExceptId);
            DeleteElements<EntitySearch>(query);
        }

        public void DeleteEntity(EntityRelated entityRelated, string id) {
            var query = string.Format(Queries(SearchQuery.GET_ELEMENT), (int)entityRelated, id);
            DeleteElements<EntitySearch>(query);
        }

        private V2.BaseProperty<T> GetProperty<T>(int index, object value) {
            var element = (V2.BaseProperty<T>)Activator.CreateInstance(typeof(V2.BaseProperty<T>));
            element.PropertyIndex = index;
            element.Value = (T)value;
            return element;
        }

        public IEnumerable<V2.RelatedId> GetArrayOfLocalRelatedIds(KeyValuePair<SearchAttribute, object> attribute) {
            var typeValue = attribute.Value.GetType();
            if (typeValue == typeof(IEnumerable<string>))
                return ((IEnumerable<string>)attribute.Value).Select(s => new V2.RelatedId { EntityIndex = attribute.Key.Index, EntityId = (string)s });
            else
                return new List<V2.RelatedId>() { new V2.RelatedId { EntityIndex = attribute.Key.Index, EntityId = (string)attribute.Value } };
        }

        public IEnumerable<V2.RelatedId> GetArrayOfRelatedIds(KeyValuePair<SearchAttribute, object> attribute) {
            if (attribute.Value.IsEnumerable()) {
                var relateds = new List<V2.RelatedId>();
                foreach (var item in (IEnumerable<string>)attribute.Value)
                    relateds.Add(new V2.RelatedId { EntityIndex = attribute.Key.Index, EntityId = item });
                return relateds;
            }
            else
                return new List<V2.RelatedId>() { new V2.RelatedId { EntityIndex = attribute.Key.Index, EntityId = (string)attribute.Value } };
        }
        
        private IEnumerable<V2.BaseProperty<T>> GetArrayOfElements<T>(KeyValuePair<SearchAttribute, object> attribute) {
            var typeValue = attribute.Value.GetType();
            if (attribute.Value.IsEnumerable())
                return ((IEnumerable<T>)attribute.Value).Select(s => GetProperty<T>(attribute.Key.Index, s));
            else
                return new List<V2.BaseProperty<T>> { GetProperty<T>(attribute.Key.Index, attribute.Value)};
        }

        private IEnumerable<V2.BaseProperty<T>> GetPropertiesObjects<T>(Related related, Dictionary<SearchAttribute, object> elements) =>
            elements.Where(s => s.Key.Related == related).SelectMany(s => GetArrayOfElements<T>(s)).ToArray();

        private V2.RelatedId[] GetReferences(Dictionary<SearchAttribute, object> elements) =>
            elements.Where(s => s.Key.Related == Related.REFERENCE).SelectMany(GetArrayOfRelatedIds).ToArray();

        private V2.RelatedId[] GetLocalReferences(Dictionary<SearchAttribute, object> elements) =>
            elements.Where(s => s.Key.Related == Related.LOCAL_REFERENCE).SelectMany(GetArrayOfLocalRelatedIds).ToArray();

        private V2.Num32Property[] GetNumProps(Dictionary<SearchAttribute, object> values) =>
            GetPropertiesObjects<int>(Related.NUM32, values).Select(s => new V2.Num32Property { 
                PropertyIndex = s.PropertyIndex,
                Value = s.Value
            }).ToArray();

        private V2.DblProperty[] GetDblProps(Dictionary<SearchAttribute, object> values) =>
            GetPropertiesObjects<double>(Related.DBL, values).Select(s => new V2.DblProperty {
                PropertyIndex = s.PropertyIndex,
                Value = s.Value
            }).ToArray();

        private V2.DtProperty[] GetDtProps(Dictionary<SearchAttribute, object> values) =>
            GetPropertiesObjects<DateTime>(Related.DATE, values).Select(s => new V2.DtProperty {
                PropertyIndex = s.PropertyIndex,
                Value = s.Value
            }).ToArray();

        private V2.EnumProperty[] GetEnumProps(Dictionary<SearchAttribute, object> values) =>
            GetPropertiesObjects<int>(Related.ENUM, values).Select(s => new V2.EnumProperty {
                PropertyIndex = s.PropertyIndex,
                Value = s.Value
            }).ToArray();

        private V2.BoolProperty[] GetBoolProps(Dictionary<SearchAttribute, object> values) =>
            GetPropertiesObjects<bool>(Related.BOOL, values).Select(s => new V2.BoolProperty {
                PropertyIndex = s.PropertyIndex,
                Value = s.Value
            }).ToArray();

        private V2.GeoProperty[] GetGeoProps(Dictionary<SearchAttribute, object> values) =>
            GetPropertiesObjects<Point>(Related.GEO, values).Select(s => new V2.GeoProperty {
                PropertyIndex = s.PropertyIndex,
                Value = GeographyPoint.Create(s.Value.Position.Latitude, s.Value.Position.Longitude)
            }).ToArray();

        private V2.Num64Property[] GetNum64Props(Dictionary<SearchAttribute, object> values) =>
            GetPropertiesObjects<long>(Related.NUM64, values).Select(s => new V2.Num64Property {
                PropertyIndex = s.PropertyIndex,
                Value = s.Value
            }).ToArray();

        private V2.StrProperty[] GetStrProps(Dictionary<SearchAttribute, object> values) =>
          GetPropertiesObjects<string>(Related.STR, values).Select(s => new V2.StrProperty {
              PropertyIndex = s.PropertyIndex,
              Value = s.Value
          }).ToArray();

        private V2.SuggestProperty[] GetSugProps(Dictionary<SearchAttribute, object> values) =>
          GetPropertiesObjects<string>(Related.SUGGESTION, values).Select(s => new V2.SuggestProperty {
              PropertyIndex = s.PropertyIndex,
              Value = s.Value
          }).ToArray();

        private V2.EntitySearchV2[] GetEntitySearch(object obj, int index, string id) {
            var list = new List<V2.EntitySearchV2>();
            var entitySearch = new V2.EntitySearchV2 {
                Id = id,
                EntityIndex = index,
                Created = DateTime.Now
            };
            var values = obj.GetPropertiesByAttributeWithValue();
            if (!values.Any())
                return Array.Empty<V2.EntitySearchV2>();
            entitySearch.NumProperties = GetNumProps(values);
            entitySearch.DoubleProperties = GetDblProps(values);
            entitySearch.DtProperties = GetDtProps(values);
            entitySearch.EnumProperties = GetEnumProps(values);
            entitySearch.BoolProperties = GetBoolProps(values);
            entitySearch.GeoProperties = GetGeoProps(values);
            entitySearch.Num64Properties = GetNum64Props(values);
            entitySearch.StringProperties = GetStrProps(values);
            entitySearch.SuggestProperties = GetSugProps(values);
            entitySearch.RelatedIds = GetReferences(values);
            var valuesWithoutProperty = obj.GetPropertiesWithoutAttributeWithValues();
            foreach (var item in valuesWithoutProperty) {
                var value = GetEntitySearch(item, 0, string.Empty);
                entitySearch.NumProperties = entitySearch.NumProperties.Union(value.SelectMany(s=>s.NumProperties)).ToArray();
                entitySearch.DoubleProperties = entitySearch.DoubleProperties.Union(value.SelectMany(s => s.DoubleProperties)).ToArray();
                entitySearch.DtProperties = entitySearch.DtProperties.Union(value.SelectMany(s => s.DtProperties)).ToArray();
                entitySearch.EnumProperties = entitySearch.EnumProperties.Union(value.SelectMany(s => s.EnumProperties)).ToArray();
                entitySearch.GeoProperties = entitySearch.GeoProperties.Union(value.SelectMany(s => s.GeoProperties)).ToArray();
                entitySearch.Num64Properties = entitySearch.Num64Properties.Union(value.SelectMany(s => s.Num64Properties)).ToArray();
                entitySearch.StringProperties = entitySearch.StringProperties.Union(value.SelectMany(s => s.StringProperties)).ToArray();
                entitySearch.BoolProperties = entitySearch.BoolProperties.Union(value.SelectMany(s => s.BoolProperties)).ToArray();
                entitySearch.SuggestProperties = entitySearch.SuggestProperties.Union(value.SelectMany(s => s.SuggestProperties)).ToArray();
                entitySearch.RelatedIds = entitySearch.RelatedIds.Union(value.SelectMany(s => s.RelatedIds)).ToArray();
            }
            var localReference = values.Where(s => s.Key.Related == Related.LOCAL_REFERENCE);
            if (localReference.Any()) {
                foreach (var item in localReference) {
                    IEnumerable<object> collection = item.Value.IsEnumerable() ? (IEnumerable<object>)item.Value : new List<object> { item.Value };
                    foreach (var childReferences in collection) {
                        var guid = Guid.NewGuid().ToString("N");
                        var localEntities = GetEntitySearch(childReferences, item.Key.Index, guid);
                        var listReferences = entitySearch.RelatedIds.ToList();
                        listReferences.Add(new V2.RelatedId { EntityId = guid, EntityIndex = item.Key.Index });
                        entitySearch.RelatedIds = listReferences.ToArray();
                        list.AddRange(localEntities);
                    }
                }
            }
            list.Add(entitySearch);
            return list.ToArray();
        }

        public V2.EntitySearchV2[] GetEntitySearch<T>(T entity)  where T : DocumentBase {
            var reference = typeof(T).GetTypeInfo().GetCustomAttribute<ReferenceSearchAttribute>(true);
            if (reference == null)
                return Array.Empty<V2.EntitySearchV2>();
            return GetEntitySearch(entity, reference.Index, entity.Id);
        }

        public V2.EntitySearchV2[] GetEntitySearchByInput<T>(T entity) where T : InputBase {
            var reference = typeof(T).GetTypeInfo().GetCustomAttribute<ReferenceSearchAttribute>(true);
            if (reference == null)
                return Array.Empty<V2.EntitySearchV2>();
            return GetEntitySearch(entity, reference.Index, entity.Id);
        }

        public object GetEntityFromSearch(V2.EntitySearchV2 entitySearch) {
            var type = GetEntityType(entitySearch.EntityIndex);
            var entity = CreateEntityInstance(type);
            type.GetProperty("Id")?.SetValue(entity, entitySearch.Id);
            var props = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop,typeof(SearchAttribute),true)).ToList();
            SearchAttribute attr;
            dynamic values;
            props.ForEach(prop => {
                attr = prop.GetCustomAttribute<SearchAttribute>(true);
                values = FormatValues(prop, GetValues(entitySearch, (int)attr.Related, attr.Index));
                prop.SetValue(entity, values);
            });
            return entity;
        }

        private object FormatValues(PropertyInfo prop, List<object> values) {
            if (!IsEnumerableProperty(prop))
                return ((IEnumerable<object>)values).FirstOrDefault();
            else {
                var propType = prop.PropertyType;
                if (propType.IsArray)
                    return CastToGenericArray(propType.GetElementType(), values);
                else
                    return CastToGenericList(propType.GetGenericArguments()[0], values);
            }
        }
        
        private List<object> GetValues(V2.EntitySearchV2 entitySearch, int typeRelated, int indexProperty) {

            //typeRelated: Representa el tipo de dato 
            /*REFERENCE = 0,
            LOCAL_REFERENCE = 1,
            STR = 2,
            SUGGESTION = 3,
            NUM64 = 4,
            NUM32 = 5,
            DBL = 6,
            BOOL = 7,
            GEO = 8,
            ENUM = 9,
            DATE = 10*/

            //indexProperty: Representa el indice en la enumeracion de propiedades correspondientes
            /*NumRelated
             StringRelated
             DoubleRelated
             BoolRelated
             DateRelated
             GeoRelated*/

            List<object> values = new List<object>();
            switch (typeRelated) {
                case 0:
                    entitySearch.RelatedIds?.ToList().FindAll(relatedId => relatedId.EntityIndex == indexProperty).ForEach(relatedId => values.Add(relatedId.EntityId));
                    break;
                case 1:
                    entitySearch.RelatedIds?.ToList().FindAll(relatedId => relatedId.EntityIndex == indexProperty).ForEach(relatedId => {
                        values.Add(GetEntityFromSearch(GetEntityV2((EntityRelated)indexProperty, relatedId.EntityId)));
                    });
                    break;
                case 2:
                    entitySearch.StringProperties?.ToList().FindAll(stringProp => stringProp.PropertyIndex == indexProperty).ForEach(stringProp => values.Add(stringProp.Value));
                    break;
                case 3:
                    entitySearch.SuggestProperties?.ToList().FindAll(suggestProp => suggestProp.PropertyIndex == indexProperty).ForEach(suggestProp => values.Add(suggestProp.Value));
                    break;
                case 4:
                    entitySearch.Num64Properties?.ToList().FindAll(longProp => longProp.PropertyIndex == indexProperty).ForEach(longProp => values.Add(longProp.Value));
                    break;
                case 5:
                    entitySearch.NumProperties?.ToList().FindAll(intProp => intProp.PropertyIndex == indexProperty).ForEach(intProp => values.Add(intProp.Value));
                    break;
                case 6:
                    entitySearch.DoubleProperties?.ToList().FindAll(doubleProp => doubleProp.PropertyIndex == indexProperty).ForEach(doubleProp => values.Add(doubleProp.Value));
                    break;
                case 7:
                    entitySearch.BoolProperties?.ToList().FindAll(boolProp => boolProp.PropertyIndex == indexProperty).ForEach(boolProp => values.Add(boolProp.Value));
                    break;
                case 8:
                    entitySearch.GeoProperties?.ToList().FindAll(geoProp => geoProp.PropertyIndex == indexProperty).ForEach(geoProp => values.Add(geoProp.Value));
                    break;
                case 9:
                    entitySearch.EnumProperties?.ToList().FindAll(enumProp => enumProp.PropertyIndex == indexProperty).ForEach(enumProp => values.Add(enumProp.Value));
                    break;
                case 10:
                    entitySearch.DtProperties?.ToList().FindAll(dateProp => dateProp.PropertyIndex == indexProperty).ForEach(dateProp => values.Add(dateProp.Value));
                    break;
            }
            return values;
        }

    }

}