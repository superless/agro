using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;
using trifenix.agro.enums.model;

namespace trifenix.agro.model.external.Input {
    public class ExecutionOrderStatusInput : InputBase {
        
        [Required]
        public ExecutionStatus ExecutionStatus { get; set; }

        public FinishStatus FinishStatus { get; set; }

        public ClosedStatus ClosedStatus { get; set; }

        public string Comment { get; set; }

        [Required, Reference(typeof(ExecutionOrder))]
        public string IdExecutionOrder { get; set; }

    }

    public class ExecutionOrderStatusSwaggerInput {

        [Required]
        public string Name { get; set; }

        [Required]
        public ExecutionStatus ExecutionStatus { get; set; }

        [Required]
        public FinishStatus FinishStatus;

        [Required]
        public ClosedStatus ClosedStatus;
        
        public string Comment { get; set; }

        [Required]
        public string IdExecutionOrder { get; set; }

    }

}