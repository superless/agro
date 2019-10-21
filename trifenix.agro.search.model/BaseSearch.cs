using Microsoft.Azure.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.search.model.@base
{
    public abstract class BaseSearch
    {
        [IsFilterable, IsSortable, IsFacetable]
        [JsonProperty(PropertyName = "created")]
        public abstract DateTime Created { get; set; }

    }
}
