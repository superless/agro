using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using System.Linq;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;

namespace trifenix.agro.search.operations {
    public class AgroSearch : IAgroSearch{

        private readonly SearchServiceClient _search;
        private readonly string _indexName;
        public AgroSearch(string SearchServiceName, string SearchServiceKey, string SearchIndexName) {
            _search = new SearchServiceClient(SearchServiceName, new SearchCredentials(SearchServiceKey));
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

        public EntitiesSearchContainer GetPaginatedEntities(Parameters parameters) {
            var SearchParameters = new SearchParameters {
                Filter = parameters.Filters.ToString(),
                SearchFields = new[] { "IdentificadorDeEntidad" },
                Top = parameters.Quantity,
                IncludeTotalResultCount = true
            };
            if (parameters.Page.HasValue && parameters.Quantity.HasValue) {
                int? skip = (parameters.Page - 1) * parameters.Quantity;
                SearchParameters.Skip = skip;
            }
            if (parameters.Desc.HasValue) {
                string order = parameters.Desc.Value ? "asc" : "desc";
                SearchParameters.OrderBy = new[] { $"IdentificadorDeEntidad {order}" };
            }
            if (string.IsNullOrWhiteSpace(parameters.TextToSearch))
                parameters.TextToSearch = null;
            EntitiesSearchContainer entitySearch = GetSearch(parameters.TextToSearch, SearchParameters);
            return entitySearch;
        }

        private EntitiesSearchContainer GetSearch(string search, SearchParameters parameters) {
            var indexClient = _search.Indexes.GetClient(_indexName);
            var result = indexClient.Documents.Search<EntitySearch>(search, parameters);
            return new EntitiesSearchContainer {
                Total = result.Count ?? 0,
                Entities = result.Results.Select(v => v.Document).ToArray()
            };
        }

        public void DeleteEntities(List<EntitySearch> entities) {
            var indexClient = _search.Indexes.GetClient(_indexName);
            var actions = entities.Select(o => IndexAction.Delete(o));
            var batch = IndexBatch.New(actions);
            indexClient.Documents.Index(batch);
        }

    }

}