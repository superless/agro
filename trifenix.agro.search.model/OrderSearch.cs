using Microsoft.Azure.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.search.model.@base;

namespace trifenix.agro.search.model
{
    public class OrderSearch : BaseSearch
    {

        public override DateTime Created { get; set; }


        [IsFilterable]
        public string OrderId { get; set; }


        [IsSearchable]
        [JsonProperty("description")]
        public string Name { get; set; }




    }
}
