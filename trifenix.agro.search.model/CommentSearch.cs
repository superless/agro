using Microsoft.Azure.Search;
using System;
using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.search.model
{

    public class CommentSearch
    {
        [Key]
        [IsFilterable]
        public string Id { get; set; }

        public DateTime Created { get; set; }



        [IsFilterable]
        public string IdUser { get; set; }

        [IsFilterable, IsFacetable]
        public int EntityIndex { get; set; }

        [IsFilterable]
        public string EntityId { get; set; }


        [IsSearchable]
        public string Comment { get; set; }




    }

}