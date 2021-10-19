using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.validation_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de bodega
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.WAREHOUSE)]
    public class WarehouseInput : LocalBase
    {
        /// <summary>
        /// Capacidad de la bodega
        /// </summary>
        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Búsqueda por referencia del centro de costos asociado
        /// </summary>
        [Required, Reference(typeof(CostCenter))]
        [ReferenceSearch(EntityRelated.COSTCENTER)]
        public string IdCostCenter { get; set; }

    }
}
