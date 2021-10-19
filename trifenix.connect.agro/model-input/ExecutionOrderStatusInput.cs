using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Estado de la orden de ejecución
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.EXECUTION_ORDER_STATUS)]
    public class ExecutionOrderStatusInput : LocalBase
    {
        
        /// <summary>
        /// Estado de la ejecución
        /// </summary>
        [Required]
        [EnumSearch(EnumRelated.EXECUTION_STATUS)]
        public ExecutionStatus ExecutionStatus { get; set; }

        /// <summary>
        /// !!
        /// </summary>
        [EnumSearch(EnumRelated.FINISH_STATUS)]
        public FinishStatus FinishStatus { get; set; }

        /// <summary>
        /// !!
        /// </summary>
        [EnumSearch(EnumRelated.CLOSED_STATUS)]
        public ClosedStatus ClosedStatus { get; set; }

        /// <summary>
        /// Comentario
        /// </summary>
        [StringSearch(StringRelated.GENERIC_COMMENT)]
        public string Comment { get; set; }

        /// <summary>
        /// Búsqueda por referencia del id de la orden de la que se desea conocer el estado
        /// </summary>
        [ReferenceSearch(EntityRelated.EXECUTION_ORDER)]
        [Required, Reference(typeof(ExecutionOrder))]
        public string IdExecutionOrder { get; set; }

    }

   

}