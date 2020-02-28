using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.enums;

namespace trifenix.agro.db.model.agro.orders
{
    [SharedCosmosCollection("agro", "ExecutionOrder")]
    public class ExecutionOrder : DocumentBase, ISharedCosmosEntity {
       


        public override string Id { get; set; }

        public string IdOrder { get; set; }

        public DosesOrder[] DosesOrder { get; set; }


        public ExecutionStatus ExecutionStatus { get; set; }

        public string[] StatusInfo { get; set; }

        public FinishStatus FinishStatus;

        public ClosedStatus ClosedStatus;

        public DateTime? InitDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string IdUserApplicator;
        public string IdNebulizer { get; set; }
        public string IdTractor { get; set; }

    }

    


    
}