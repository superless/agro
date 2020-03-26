using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.enums;
using trifenix.agro.enums.model;

namespace trifenix.agro.model.external.Input
{
    public class ExecutionOrderStatusInput : InputBase {
        
        public ExecutionStatus ExecutionStatus { get; set; }

        public FinishStatus FinishStatus;

        public ClosedStatus ClosedStatus;


        public string Comment { get; set; }

        public string IdExecutionOrder { get; set; }

    }

    public class ExecutionOrderStatusSwaggerInput 
    {

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