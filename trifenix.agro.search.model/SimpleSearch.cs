using Microsoft.Azure.Search;
using System;
using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.search.model
{
    public class SimpleSearch {

        
        [Key]
        [IsFilterable]
        public string Id { get; set; }

        public DateTime Created { get; set; }


        [IsSearchable, IsSortable, IsFilterable]
        public string Name { get; set; }

        [IsSearchable, IsSortable, IsFilterable]
        public string Abbreviation { get; set; }

        [IsFilterable]
        public string EntityName { get; set; }

    }

}