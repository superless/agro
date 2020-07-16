using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.db;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.model
{
    /// <summary>
    /// Sector de una parcela.
    /// </summary>
    [SharedCosmosCollection("agro", "Sector")]
    [ReferenceSearchHeader(EntityRelated.SECTOR, PathName = "sectors", Kind = EntityKind.ENTITY)]
    [GroupMenu(MenuEntityRelated.MANTENEDORES, PhisicalDevice.ALL, SubMenuEntityRelated.TERRENO)]
    public class Sector : DocumentBaseName<long>, ISharedCosmosEntity
    {
        /// <summary>
        /// identificador
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// nombre
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }



    }


}
