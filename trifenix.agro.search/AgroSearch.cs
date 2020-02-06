using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using System.Linq;
using trifenix.agro.search.model;

namespace trifenix.agro.search {
    public class AgroSearch {

        private SearchServiceClient _search;
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

        public EntitiesSearchContainer GetSearchFilteredByEntityName(Filters filters, string search, int? page, int? quantity, bool? desc) {
            var parameters = new SearchParameters {
                Filter = filters.ToString(),
                SearchFields = new[] { "IdentificadorDeEntidad" },
                Top = quantity,
                IncludeTotalResultCount = true
            };
            if(page.HasValue && quantity.HasValue){
                int? skip = (page - 1) * quantity;
                parameters.Skip = skip;
            }
            if (desc.HasValue) {
                string order = desc.Value?"desc":"asc";
                parameters.OrderBy = new[] { $"IdentificadorDeEntidad {order}" };
            }
            if (string.IsNullOrWhiteSpace(search))
                search = null;
            return GetSearch(search, parameters);
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

    public class Filters {

        public string EntityName { get; set; }
        public string SeasonId { get; set; }
        public int? Status { get; set; }
        public bool? Type { get; set; }
        public override string ToString() => $"EntityName eq '{EntityName}'" + (!string.IsNullOrWhiteSpace(SeasonId)?$" and SeasonId eq '{SeasonId}'":"") + (Status.HasValue?$" and Status eq {Status}":"") + (Type.HasValue?" and " + (Type.Value?"Type": "not Type"):"");

    }

}