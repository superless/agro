using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using System.Linq;
using trifenix.agro.enums;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.search.operations {
    public class AgroSearch : IAgroSearch{

        private readonly SearchServiceClient _search;
        private readonly string _entityIndex = "entities";
        private readonly string _commentIndex = "comments";

        public AgroSearch(string SearchServiceName, string SearchServiceKey) {
            _search = new SearchServiceClient(SearchServiceName, new SearchCredentials(SearchServiceKey));
            if (!_search.Indexes.Exists(_entityIndex))
                _search.Indexes.CreateOrUpdate(new Index { Name = _entityIndex, Fields = FieldBuilder.BuildForType<EntitySearch>() });
            if (!_search.Indexes.Exists(_commentIndex))
                 _search.Indexes.CreateOrUpdate(new Index { Name = _commentIndex, Fields = FieldBuilder.BuildForType<CommentSearch>() });
        }

        private void OperationElements<T>(List<T> elements, SearchOperation operationType) {

            var indexName = typeof(T).Equals(typeof(EntitySearch)) ? _entityIndex : _commentIndex;
            var indexClient = _search.Indexes.GetClient(indexName);
            var actions = elements.Select(o => operationType == SearchOperation.Add ? IndexAction.MergeOrUpload(o) : IndexAction.Delete(o));
            var batch = IndexBatch.New(actions);
            indexClient.Documents.Index(batch);
        }

        public void AddElements<T>(List<T> elements) {
            OperationElements(elements, SearchOperation.Add);
        }

        public void DeleteElements<T>(List<T> elements) {
            OperationElements(elements, SearchOperation.Delete);
        }

    }

}