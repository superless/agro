using System;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de orden de ejecución
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.EXECUTION_ORDER)]
    public class ExecutionOrderInput : LocalBase
    {

        /// <summary>
        /// Búsqueda por referencia del id de la órden a ejecutar
        /// </summary>
        [ReferenceSearch(EntityRelated.ORDER)]
        [Required, Reference(typeof(ApplicationOrder))]
        public string IdOrder { get; set; }

        [ReferenceSearch(EntityRelated.DOSES_ORDER, true)]
        //TODO: Cristian esto válida la existencia de la dosis dentro del objeto DosesOrder.
        [Required, Reference(typeof(Dose))]
        public DosesOrder[] DosesOrder { get; set; }
        
        /// <summary>
        /// Fecha de inicio
        /// </summary>
        [DateSearch(DateRelated.START_DATE_EXECUTION_ORDER)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Fecha de término
        /// </summary>
        [DateSearch(DateRelated.END_DATE_EXECUTION_ORDER)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Búsqueda por referencia del usuario que ejecutará la orden
        /// </summary>
        [ReferenceSearch(EntityRelated.USER)]
        [Reference(typeof(UserApplicator))]
        public string IdUserApplicator { get; set; }

        /// <summary>
        /// Búsqueda por referencia del nebulizador a utilizar
        /// </summary>
        [ReferenceSearch(EntityRelated.NEBULIZER)]
        [Reference(typeof(Nebulizer))]
        public string IdNebulizer { get; set; }

        /// <summary>
        /// Búsqueda por referencia del tractor a utilizar
        /// </summary>
        [ReferenceSearch(EntityRelated.TRACTOR)]
        [Reference(typeof(Tractor))]
        public string IdTractor { get; set; }

    }


}