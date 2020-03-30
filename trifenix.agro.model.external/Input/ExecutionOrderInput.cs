using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.db.model;

using trifenix.agro.db.model.orders;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

    [ReferenceSearch(EntityRelated.EXECUTION_ORDER)]
    public class ExecutionOrderInput : InputBase {

        [ReferenceSearch(EntityRelated.ORDER)]
        [Required, Reference(typeof(ApplicationOrder))]
        public string IdOrder { get; set; }

        [ReferenceSearch(EntityRelated.DOSES_ORDER, true)]


        //TODO: Cristian esto válida la existencia de la dosis dentro del objeto DosesOrder.
        [Required, Reference(typeof(Dose))]
        public DosesOrder[] DosesOrder { get; set; }

        [DateSearch(DateRelated.START_DATE)]
        public DateTime? StartDate { get; set; }

        [DateSearch(DateRelated.END_DATE)]
        public DateTime? EndDate { get; set; }


        [ReferenceSearch(EntityRelated.USER)]
        [Reference(typeof(UserApplicator))]
        public string IdUserApplicator { get; set; }

        [ReferenceSearch(EntityRelated.NEBULIZER)]
        [Reference(typeof(Nebulizer))]
        public string IdNebulizer { get; set; }


        [ReferenceSearch(EntityRelated.TRACTOR)]
        [Reference(typeof(Tractor))]
        public string IdTractor { get; set; }

    }


}