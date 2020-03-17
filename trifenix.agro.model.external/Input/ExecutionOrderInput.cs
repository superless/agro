using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.orders;

namespace trifenix.agro.model.external.Input {

    public class ExecutionOrderInput : InputBase {

        [Required, Reference(typeof(ApplicationOrder))]
        public string IdOrder { get; set; }

        [Required, Reference(typeof(Dose))]
        public DosesOrder[] DosesOrder { get; set; }

        public DateTime? InitDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Reference(typeof(UserApplicator))]
        public string IdUserApplicator { get; set; }

        [Reference(typeof(Nebulizer))]
        public string IdNebulizer { get; set; }

        [Reference(typeof(Tractor))]
        public string IdTractor { get; set; }

    }

    public class ExecutionOrderSwaggerInput : InputBase {

        [Required]
        public string IdOrder { get; set; }

        public DosesOrder[] DosesOrder { get; set; }

        public DateTime? InitDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string IdUserApplicator { get; set; }

        public string IdNebulizer { get; set; }

        public string IdTractor { get; set; }

    }

}