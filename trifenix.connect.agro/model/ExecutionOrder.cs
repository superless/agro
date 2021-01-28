
using Cosmonaut.Attributes;
using System;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{


    /// <summary>
    /// Ejecución de una orden.
    /// </summary>
    [SharedCosmosCollection("agro", "ExecutionOrder")]
    [ReferenceSearchHeader(EntityRelated.EXECUTION_ORDER, Kind = EntityKind.PROCESS, PathName = "execution_orders")]
    public class ExecutionOrder : DocumentLocal {


        /// <summary>
        /// Identificador de la orden de ejecución.
        /// </summary>
        public override string Id { get; set; }


        /// <summary>
        /// Autonumérico.
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        /// <summary>
        /// Orden relacionada a la ejeución.
        /// </summary>
        [ReferenceSearch(EntityRelated.ORDER)]
        public string IdOrder { get; set; } // orden


        /// <summary>
        /// Dosis asociadas, 
        /// esto se cambia solo si el resultado final es incompleto.
        /// </summary>
        [ReferenceSearch(EntityRelated.DOSES_ORDER, true)]
        public DosesOrder[] DosesOrder { get; set; } // se modifica solo si, lo determina el agronomo, al finalizar y poner incompleto.

        /// <summary>
        /// Fecha inicio que asignó el operador.
        /// </summary>
        [DateSearch(DateRelated.START_DATE_EXECUTION_ORDER)]
        public DateTime? StartDate { get; set; } // fecha de inicio que el agronomo puede determinar.


        /// <summary>
        /// Fecha de fin que asignó el operador.
        /// </summary>
        [DateSearch(DateRelated.END_DATE_EXECUTION_ORDER)]
        public DateTime? EndDate { get; set; } // fecha de fin que el agronomo determina

        /// <summary>
        /// Aplicador
        /// </summary>
        [ReferenceSearch(EntityRelated.USER)]
        public string IdUserApplicator { get; set; } // id user applicator


        /// <summary>
        /// Nebulizadora asignada
        /// </summary>
        [ReferenceSearch(EntityRelated.NEBULIZER)]
        public string IdNebulizer { get; set; } // nebulizadora

        /// <summary>
        /// Identificador del tractor.
        /// </summary>
        [ReferenceSearch(EntityRelated.TRACTOR)]
        public string IdTractor { get; set; } // tractor

    }

}