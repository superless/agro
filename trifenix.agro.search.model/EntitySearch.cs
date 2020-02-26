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

        public IdsRelated[] IdsRelated { get; set; } 
        
        public ElementRelated[] ElementsRelated { get; set; }       //Orden, Ejecucion, PreOrden, 

        public NumberEntityRelated[] NumbersRelated { get; set; }

    }

    public class IdsRelated {

        [IsFilterable, IsSortable]
        public int EntityIndex { get; set; }

        [IsFilterable, IsSortable]
        public string EntityId { get; set; }

    }

    public class ElementRelated {

        [IsFilterable, IsSortable]
        public int EntityIndex { get; set; }

        [IsSearchable, IsSortable]
        public string Name { get; set; }

    }

    public class NumberEntityRelated {

        [IsFilterable, IsSortable]
        public int EntityIndex { get; set; }

        [IsFilterable, IsSortable]
        public int Number { get; set; }

    }

}