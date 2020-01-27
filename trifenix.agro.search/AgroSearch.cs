using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using System.Linq;
using trifenix.agro.search.model;

namespace trifenix.agro.search {
    public class AgroSearch {

        private SearchServiceClient _search;
        private readonly string _indexName;
        public readonly string _entityName;
        public AgroSearch(string SearchServiceName, string SearchServiceKey, string SearchIndexName, string EntityName) {
            _search = new SearchServiceClient(SearchServiceName, new SearchCredentials(SearchServiceKey));
            _entityName = EntityName;
            _indexName = SearchIndexName;
            if (_search.Indexes.Exists(_indexName))
                return;
            _search.Indexes.CreateOrUpdate(new Index { Name = _indexName, Fields = FieldBuilder.BuildForType<EntitySearch>() });
        }
        

        public void AddEntities(List<EntitySearch> entities) {
            var indexClient = _search.Indexes.GetClient(_indexName);
            var actions = entities.Select(o => IndexAction.MergeOrUpload(o));
            var batch = IndexBatch.New(actions);
            indexClient.Documents.Index(batch);
        }

        public EntitiesSearchContainer GetSearchFilteredByEntityName(string search, int page, int quantity, bool desc) {
            var skip = (page - 1) * quantity;
            var indexClient = _search.Indexes.GetClient(_indexName);
            var order = desc ? "desc" : "asc";
            var result = indexClient.Documents.Search<EntitySearch>(!string.IsNullOrWhiteSpace(search)?search:null, new SearchParameters {
                Filter = $"EntityName eq '{_entityName}'",
                SearchFields = new [] { "IdentificadorDeEntidad" },
                Skip = skip,
                Top = quantity,
                IncludeTotalResultCount = true,                
                OrderBy = new[] { $"IdentificadorDeEntidad {order}" }
            });
            return new EntitiesSearchContainer {
                Total = result.Count ?? 0,
                Entities = result.Results.Select(v => v.Document).ToArray()
            };
        }

    }
}
