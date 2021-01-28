using Cosmonaut.Attributes;
using System;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{

    [SharedCosmosCollection("agro", "ExecutionOrderStatus")]
    [ReferenceSearchHeader(EntityRelated.EXECUTION_ORDER_STATUS, Kind = EntityKind.CUSTOM_ENTITY)]
    public class ExecutionOrderStatus : DocumentLocal {


        /// <summary>
        /// Identificador de la ejecución.
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Autonumérico del identificador del cliente.
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        /// <summary>
        /// Fecha de creación, la último estado de una orden, determina el estado actual.
        /// </summary>
        public DateTime Created { get; set; }  // fecha de creacion     

        /// <summary>
        /// Estatus de la ejecución.
        /// esto determina el status actual de la ejecución.
        /// </summary>
        [EnumSearch(EnumRelated.EXECUTION_STATUS)]
        public ExecutionStatus ExecutionStatus { get; set; } // etapas del proceso


        /// <summary>
        /// Determina el estado asignado por el bodeguero al momento de recibir al operador. 
        /// </summary>
        [EnumSearch(EnumRelated.FINISH_STATUS)]
        public FinishStatus FinishStatus { get; set; }

        /// <summary>
        /// Cierro de la orden.
        /// </summary>
        [EnumSearch(EnumRelated.CLOSED_STATUS)]
        public ClosedStatus ClosedStatus { get; set; }


        /// <summary>
        /// Comentario del estado.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_COMMENT)]
        public string Comment { get; set; }

        /// <summary>
        /// Id de la orden de ejecucíón asociada.
        /// </summary>
        [ReferenceSearch(EntityRelated.EXECUTION_ORDER)]
        public string IdExecutionOrder { get; set; }


    }

}