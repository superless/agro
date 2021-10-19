using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm.validation_attributes;
using trifenix.connect.mdm_attributes;
using trifenix.connect.model;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Sector de una parcela.
    /// </summary>
    
    [ReferenceSearchHeader(EntityRelated.SECTOR, PathName = "sectors", Kind = EntityKind.ENTITY)]
    [GroupMenu("Principales", PhisicalDevice.ALL, "Ubicación")]
    public class Sector : DocumentDb
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        [Unique]
        public string Name { get; set; }

        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }



    }


}
