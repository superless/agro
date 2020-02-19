using Microsoft.Azure.Search;
using Newtonsoft.Json;
using System;

namespace trifenix.agro.search.model.@base {
    public abstract class BaseSearch {
        [IsFilterable, IsSortable, IsFacetable]
        [JsonProperty(PropertyName = "created")]
        public abstract DateTime Created { get; set; }

    }
}