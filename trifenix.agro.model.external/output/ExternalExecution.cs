using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;
using trifenix.agro.enums.model;

namespace trifenix.agro.model.external.output
{
    public class ExternalExecution
    {
        public string Id { get; set; }

        public string IdApplicator { get; set; }

        public string IdOrder { get; set; }

        public string IdTractor { get; set; }

        public string IdNubulizer { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ExternalExectionApplicationInOrder[] productToApply { get; set; }

        public ExecutionStatus ExecutionStatus { get; set; }

        public FinishStatus FinishStatus { get; set; }

        public ClosedStatus ClosedStatus { get; set; }


        public string[] StatusInfo { get; set; }

        public string[] Comments { get; set; }




    }
}
