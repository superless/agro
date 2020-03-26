using Microsoft.Azure.Search;
using System;
using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.search.model {

    public class EntitySearch {

        [Key]
        [IsFilterable]
        public string Id { get; set; }                              

        [IsFilterable, IsFacetable]
        public int EntityIndex { get; set; }                        

        [IsSortable]
        public DateTime Created { get; set; }                       
            

        
        public RelatedId[] RelatedIds { get; set; } 
        
        public Property[] RelatedProperties { get; set; }       

        public RelatedEnumValue[] RelatedEnumValues { get; set; }

    }

    public class RelatedId {

        [IsFilterable, IsFacetable]
        public int EntityIndex { get; set; }

        [IsFilterable]
        public string EntityId { get; set; }

        [IsFilterable, IsFacetable]
        public string Id { get { return $"{EntityIndex},{EntityId}"; } }

    }

    public class Property {

        [IsFilterable, IsFacetable]
        public int PropertyIndex { get; set; }

        [IsSearchable]
        public string Value { get; set; }

        

        [IsFilterable, IsFacetable]
        public string Id { get { return $"{PropertyIndex},{Value}";  } }



    }

    public class RelatedEnumValue {

        [IsFilterable, IsFacetable]
        public int EnumerationIndex { get; set; }

        [IsFilterable]
        public int Value { get; set; }

        [IsFilterable, IsFacetable]
        public string Id { get { return $"{EnumerationIndex},{Value}"; } }
    }

}