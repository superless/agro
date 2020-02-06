using Microsoft.Azure.Search;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.search.model.@base;

namespace trifenix.agro.search.model {
    public class EntitySearch : BaseSearch {

        public override DateTime Created { get; set; }

        [IsFilterable]
        public string EntityName { get; set; }

        [Key]
        [IsFilterable]
        public string Id { get; set; }

        [IsSearchable, IsSortable, JsonProperty("IdentificadorDeEntidad")]
        public string Name { get; set; }

        [IsSortable, IsFilterable]
        public string SeasonId { get; set; }

        [IsSortable, IsFilterable]
        public int Status { get; set; }
        
        [IsFilterable]
        public bool Type { get; set; }
    }

    public class EntitiesSearchContainer {

        public EntitySearch[] Entities { get; set; }
        public long Total { get; set; }
    }
}