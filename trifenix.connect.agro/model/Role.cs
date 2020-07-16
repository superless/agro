using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.db;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.model
{

    [SharedCosmosCollection("agro", "Role")]
    [ReferenceSearchHeader(EntityRelated.ROLE, Kind = EntityKind.ENTITY, PathName = "roles")]
    [GroupMenu(MenuEntityRelated.MANTENEDORES, PhisicalDevice.ALL, SubMenuEntityRelated.USUARIOS)]
    public class Role : DocumentBaseName<long>, ISharedCosmosEntity
    {
        /// <summary>
        /// identificador del rol
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }

        /// <summary>
        /// nombre del rol
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

    }
}
