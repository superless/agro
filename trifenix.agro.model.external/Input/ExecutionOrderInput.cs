using System;
using trifenix.agro.db.model.agro.orders;

namespace trifenix.agro.model.external.Input
{

    public class ExecutionOrderInput : InputBase
    {

        public string IdOrder { get; set; }

        public DosesOrder[] DosesOrder { get; set; }

      
        public DateTime? InitDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string IdUserApplicator;
        public string IdNebulizer { get; set; }
        public string IdTractor { get; set; }

        
        
    }




}