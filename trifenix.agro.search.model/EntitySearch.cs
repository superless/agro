using Microsoft.Azure.Search;
using System;
using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.search.model {

    public class EntitySearch {

        [Key]
        [IsFilterable]
        public string Id { get; set; }                              //Todos

        [IsFilterable]
        public int EntityIndex { get; set; }                        //Todos

        [IsSortable]
        public DateTime Created { get; set; }                       //Todos

        public RelatedId[] RelatedIds { get; set; } 
        
        public Property[] RelatedProperties { get; set; }       //Orden, Ejecucion, PreOrden, 

        public RelatedEnumValue[] RelatedEnumValues { get; set; }

    }

    public class RelatedId {

        [IsFilterable, IsSortable]
        public int EntityIndex { get; set; }

        [IsFilterable, IsSortable, IsSearchable]
        public string EntityId { get; set; }

        


    }

    public class Property {

        [IsFilterable, IsSortable]
        public int PropertyIndex { get; set; }

        [IsSearchable, IsSortable]
        public string Value { get; set; }



    }

    public class RelatedEnumValue {

        [IsFilterable, IsSortable]
        public int EnumerationIndex { get; set; }

        [IsFilterable, IsSortable]
        public int Value { get; set; }

    }

}