using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using trifenix.agro.enums;

namespace trifenix.agro.db.model.agro.orders
{
    [SharedCosmosCollection("agro", "ExecutionOrderStatus")]
    public class ExecutionOrderStatus : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }
        public DateTime Created { get; set; }        
        public ExecutionStatus ExecutionStatus { get; set; }

        public FinishStatus FinishStatus;

        public ClosedStatus ClosedStatus;
        public string Comment { get; set; }

        public string IdExecutionOrder { get; set; }






    }

    


    
}