using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.search.model;

namespace trifenix.agro.search
{
    public class AgroSearch
    {
        private SearchServiceClient _search;

        public string IndexOrder { get { return "orders"; } }

        public AgroSearch(string name, string key)
        {
            _search = new SearchServiceClient(name, new SearchCredentials(key));
            if (_search.Indexes.Exists(IndexOrder)) return;
            _search.Indexes.CreateOrUpdate(new Index { Name = IndexOrder, Fields = FieldBuilder.BuildForType<OrderSearch>() });
        }
        

        public void AddOrders(List<OrderSearch> orders) {
            var indexClient = _search.Indexes.GetClient(IndexOrder);
            var actions = orders.Select(o => IndexAction.MergeOrUpload(o));
            var batch = IndexBatch.New(actions);

            indexClient.Documents.Index(batch);
        }

        public OrderSearchContainer GetOrders(string search, int page, int quantity, bool desc ) {

            var skip = (page - 1) * quantity;
            var indexClient = _search.Indexes.GetClient(IndexOrder);
            var order = desc ? "desc" : "asc";
            var result = indexClient.Documents.Search<OrderSearch>(!string.IsNullOrWhiteSpace(search)?search:null, new SearchParameters {
                SearchFields = new List<string> { "description" },
                Skip = skip == 0 ? 0 : skip,
                Top = quantity,
                IncludeTotalResultCount = true,
                
                OrderBy = new List<string>() { $"description {order}" }

            }); ; ;

            return new OrderSearchContainer
            {
                Total = result.Count ?? 0,
                Orders = result.Results.Select(v => v.Document).ToArray()

            };
        }


    }
}
