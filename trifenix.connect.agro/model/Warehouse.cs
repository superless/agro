using Cosmonaut.Attributes;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.model
{
    /// <summary>
    /// Bodega encargada del almacenamiento de productos
    /// </summary>
    [SharedCosmosCollection("agro", "Warehouse")]
    [ReferenceSearchHeader(EntityRelated.WAREHOUSE, PathName = "warehouse", Kind = EntityKind.ENTITY)]
    public class Warehouse : DocumentLocal
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Capacidad de la bodega
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Búsqueda por referecia del centro de costos asociado
        /// </summary>
        [ReferenceSearch(EntityRelated.COSTCENTER)]
        public string IdCostCenter { get; set; }

        /// <summary>
        /// Identificador visual de la bodega
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }
    }
}
