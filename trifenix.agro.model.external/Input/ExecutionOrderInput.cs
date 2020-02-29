using System;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;

namespace trifenix.agro.model.external.Input
{
    public class ExecutionOrderInput : InputBase
    {

        public string IdOrder { get; set; }

        public DosesOrder[] DosesOrder { get; set; }

        public ExecutionStatus ExecutionStatus { get; set; }

        public FinishStatus FinishStatus;

        public ClosedStatus ClosedStatus;

        public DateTime? InitDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string IdUserApplicator;
        public string IdNebulizer { get; set; }
        public string IdTractor { get; set; }

        public string StatusCommentary { get; set; }

        
    }




}