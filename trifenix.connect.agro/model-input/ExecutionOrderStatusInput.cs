using System.ComponentModel.DataAnnotations;
using trifenix.agro.model.external.Input;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.Validations;

namespace trifenix.connect.agro_model_input
{

    [ReferenceSearchHeader(EntityRelated.EXECUTION_ORDER_STATUS)]
    public class ExecutionOrderStatusInput : InputBase {
        
        [Required]
        [EnumSearch(EnumRelated.EXECUTION_STATUS)]
        public ExecutionStatus ExecutionStatus { get; set; }

        [EnumSearch(EnumRelated.FINISH_STATUS)]
        public FinishStatus FinishStatus { get; set; }

        [EnumSearch(EnumRelated.CLOSED_STATUS)]
        public ClosedStatus ClosedStatus { get; set; }

        [StringSearch(StringRelated.GENERIC_COMMENT)]
        public string Comment { get; set; }


        [ReferenceSearch(EntityRelated.EXECUTION_ORDER)]
        [Required, Reference(typeof(ExecutionOrder))]
        public string IdExecutionOrder { get; set; }

    }

   

}