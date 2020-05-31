using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.db.model.orders;
using trifenix.agro.enums;
using trifenix.agro.enums.model;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

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