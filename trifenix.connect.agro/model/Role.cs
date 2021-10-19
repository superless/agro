using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;
using trifenix.connect.model;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Entidad encargada de generar los roles
    /// </summary>
    
    [ReferenceSearchHeader(EntityRelated.ROLE, Kind = EntityKind.ENTITY, PathName = "roles")]
    [GroupMenu("Complementarios", PhisicalDevice.ALL, "Usuarios")]
    public class Role : DocumentDb
    {
        /// <summary>
        /// Identificador del rol
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        /// <summary>
        /// nombre del rol
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

    }
}
