using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using System.Linq;
using trifenix.agro.db;
using trifenix.agro.enums;
using trifenix.agro.enums.query;
using trifenix.agro.enums.search;
using trifenix.agro.enums.searchModel;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.search.operations {
    
    public class AgroSearch : IAgroSearch {
    
        private readonly SearchServiceClient _search;
        private readonly string _entityIndex = "entities";
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
                _search.Indexes.CreateOrUpdate(new Index { Name = _entityIndex, Fields = FieldBuilder.BuildForType<EntitySearch>() });
            if (!_search.Indexes.Exists(_commentIndex))
                _search.Indexes.CreateOrUpdate(new Index { Name = _commentIndex, Fields = FieldBuilder.BuildForType<CommentSearch>() });
        }

        private string Queries(SearchQuery query) => _queries.Get(query);

        private void OperationElements<T>(List<T> elements, SearchOperation operationType) {
            var indexName = typeof(T).Equals(typeof(EntitySearch)) ? _entityIndex : _commentIndex;
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

        public EntitySearch[] GetEntitySearch<T>(T model)  where T : DocumentBase {
            throw new System.NotImplementedException();
        }

    }

}