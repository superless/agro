using System;
using trifenix.agro.enums;

namespace trifenix.agro.model.external.Input
{
    public class ExecutionOrderStatusInput : InputBase {
        public DateTime Created { get; set; }
        public ExecutionStatus ExecutionStatus { get; set; }

        public FinishStatus FinishStatus;

        public ClosedStatus ClosedStatus;
        public string Comment { get; set; }

        public string IdExecutionOrder { get; set; }

    }




}