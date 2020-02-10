using Microsoft.Azure.Search;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.search.model.@base;

namespace trifenix.agro.search.model {
    public class EntitySearch : BaseSearch {

        public override DateTime Created { get; set; }  //Todos

        [IsFilterable]
        public string EntityName { get; set; }          //Todos

        [Key]
        [IsFilterable]
        public string Id { get; set; }                  //Todos

        [IsSearchable, IsSortable, JsonProperty("IdentificadorDeEntidad")]
        public string Name { get; set; }                //Todos

        [IsSortable, IsFilterable]
        public string SeasonId { get; set; }            //Orden, Ejecucion, Barrack

        [IsSortable, IsFilterable]
        public int? Status { get; set; }                //Ejecucion

        [IsFilterable]
        public bool? Type { get; set; }                 //Orden

        [IsFilterable]
        public string Specie { get; set; }              //Orden, Ejecucion, Barrack
    }

    public class EntitiesSearchContainer {

        public EntitySearch[] Entities { get; set; }
        public long Total { get; set; }
    }

    public class Filters {

        public string EntityName { get; set; }
        public string SeasonId { get; set; }
        public int? Status { get; set; }
        public bool? Type { get; set; }
        public string Specie { get; set; }
        public override string ToString() => $"EntityName eq '{EntityName}'" + (!string.IsNullOrWhiteSpace(SeasonId) ? $" and SeasonId eq '{SeasonId}'" : "") + (Status.HasValue ? $" and Status eq {Status}" : "") + (Type.HasValue ? (" and " + (Type.Value ? "Type" : "not Type")) : "") + (!string.IsNullOrWhiteSpace(Specie) ? $" and Specie eq '{Specie}'" : "");

    }

    public class Parameters {

        public string TextToSearch { get; set; }
        public Filters Filters { get; set; }
        public int? Page { get; set; }
        public int? Quantity { get; set; }
        public bool? Desc { get; set; }

    }

}