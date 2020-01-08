using Microsoft.Azure.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using trifenix.agro.search.model.@base;

namespace trifenix.agro.search.model
{
    public class OrderSearch : BaseSearch
    {

        public override DateTime Created { get; set; }

        [Key]
        [IsFilterable]
        public string OrderId { get; set; }


        [IsSearchable]
        [IsSortable]
        [JsonProperty("description")]
        public string Name { get; set; }
    }

    public class OrderSearchContainer {

        public OrderSearch[] Orders { get; set; }

        public long Total { get; set; }


    }
}
