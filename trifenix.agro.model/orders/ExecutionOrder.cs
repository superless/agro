using Cosmonaut;
using Cosmonaut.Attributes;
using System;

namespace trifenix.agro.db.model.agro.orders {

    [SharedCosmosCollection("agro", "ExecutionOrder")]
    public class ExecutionOrder : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }
        
        public string IdOrder { get; set; } // orden
        public DosesOrder[] DosesOrder { get; set; } // se modifica solo si, lo determina el agronomo, al finalizar y poner incompleto.
        public DateTime? InitDate { get; set; } // fecha de inicio que el agronomo puede determinar. 
        public DateTime? EndDate { get; set; } // fecha de fin que el agronomo determina
        public string IdUserApplicator { get; set; } // id user applicator
        public string IdNebulizer { get; set; } // nebilizadora
        public string IdTractor { get; set; } // tractor

    }

}