using Microsoft.Azure.Search;
using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.search.model.@base;

namespace trifenix.agro.search.model
{

    public class CommentSearch : BaseSearch
    {
        [Key]
        [IsFilterable]
        public string Id { get; set; }


        
        public override DateTime Created { get; set; }



        [IsSearchable, IsSortable, IsFilterable]
        public string UserName { get; set; }

        [IsFilterable]
        public string EntityName { get; set; }

        [IsFilterable]
        public string IdEntityName { get; set; }


        [IsSearchable]
        public string Comment { get; set; }




    }

}