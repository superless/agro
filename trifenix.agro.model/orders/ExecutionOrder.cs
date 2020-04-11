using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.orders {

    [SharedCosmosCollection("agro", "ExecutionOrder")]
    [ReferenceSearch(EntityRelated.EXECUTION_ORDER)]
    public class ExecutionOrder : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }

        [ReferenceSearch(EntityRelated.ORDER)]
        public string IdOrder { get; set; } // orden

        [ReferenceSearch(EntityRelated.DOSES_ORDER, true)]
        public DosesOrder[] DosesOrder { get; set; } // se modifica solo si, lo determina el agronomo, al finalizar y poner incompleto.

        [DateSearch(DateRelated.START_DATE_EXECUTION_ORDER)]
        public DateTime? StartDate { get; set; } // fecha de inicio que el agronomo puede determinar.

        [DateSearch(DateRelated.END_DATE_EXECUTION_ORDER)]
        public DateTime? EndDate { get; set; } // fecha de fin que el agronomo determina

        [ReferenceSearch(EntityRelated.USER)]
        public string IdUserApplicator { get; set; } // id user applicator

        [ReferenceSearch(EntityRelated.NEBULIZER)]
        public string IdNebulizer { get; set; } // nebulizadora

        [ReferenceSearch(EntityRelated.TRACTOR)]
        public string IdTractor { get; set; } // tractor

    }

}