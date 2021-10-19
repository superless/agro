using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.validation_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de centro de costos
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.COSTCENTER)]
    public class CostCenterInput : LocalBase {

        /// <summary>
        /// Nombre 
        /// </summary>
        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Búsqueda por referencia del id del nombre del negocio
        /// </summary>
        [Required,Reference(typeof(BusinessName))]
        [ReferenceSearch(EntityRelated.BUSINESSNAME)]
        public string IdBusinessName { get; set; }

    }
}