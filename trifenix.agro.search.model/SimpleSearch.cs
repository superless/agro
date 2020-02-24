using Microsoft.Azure.Search;
using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.search.model.@base;

namespace trifenix.agro.search.model
{
    public class SimpleSearch : BaseSearch {

        
        [Key]
        [IsFilterable]
        public string Id { get; set; }

        public override DateTime Created { get; set; }


        [IsSearchable, IsSortable, IsFilterable]
        public string Name { get; set; }

        [IsSearchable, IsSortable, IsFilterable]
        public string Abbreviation { get; set; }

        [IsFilterable]
        public string EntityName { get; set; }




    }

}