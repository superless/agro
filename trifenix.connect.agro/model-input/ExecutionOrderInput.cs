using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.model.external.Input;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;

namespace trifenix.connect.agro_model_input
{

    [ReferenceSearchHeader(EntityRelated.EXECUTION_ORDER)]
    public class ExecutionOrderInput : InputBase {

        [ReferenceSearch(EntityRelated.ORDER)]
        [Required, Reference(typeof(ApplicationOrder))]
        public string IdOrder { get; set; }

        [ReferenceSearch(EntityRelated.DOSES_ORDER, true)]
        //TODO: Cristian esto válida la existencia de la dosis dentro del objeto DosesOrder.
        [Required, Reference(typeof(Dose))]
        public DosesOrder[] DosesOrder { get; set; }

        [DateSearch(DateRelated.START_DATE_EXECUTION_ORDER)]
        public DateTime? StartDate { get; set; }

        [DateSearch(DateRelated.END_DATE_EXECUTION_ORDER)]
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