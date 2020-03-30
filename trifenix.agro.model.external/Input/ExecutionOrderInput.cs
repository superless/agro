using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model;
using trifenix.agro.db.model;
using trifenix.agro.db.model.orders;

namespace trifenix.agro.model.external.Input {

    public class ExecutionOrderInput : InputBase {

        [Required, ReferenceAttribute(typeof(ApplicationOrder))]
        public string IdOrder { get; set; }

        [Required, ReferenceAttribute(typeof(Dose))]
        public DosesOrder[] DosesOrder { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [ReferenceAttribute(typeof(UserApplicator))]
        public string IdUserApplicator { get; set; }

        [ReferenceAttribute(typeof(Nebulizer))]
        public string IdNebulizer { get; set; }

        [ReferenceAttribute(typeof(Tractor))]
        public string IdTractor { get; set; }

    }

    public class ExecutionOrderSwaggerInput : InputBase {

        [Required]
        public string IdOrder { get; set; }

        public DosesOrder[] DosesOrder { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string IdUserApplicator { get; set; }

        public string IdNebulizer { get; set; }

        public string IdTractor { get; set; }

    }

}