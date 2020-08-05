using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.db;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{

    /// <summary>
    /// raíz de una planta.
    /// </summary>
    [SharedCosmosCollection("agro", "Rootstock")]
    [ReferenceSearchHeader(EntityRelated.ROOTSTOCK, PathName = "rootstock", Kind = EntityKind.ENTITY)]
    public class Rootstock : DocumentBaseName, ISharedCosmosEntity {


        /// <summary>
        /// identificador
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// nombre del rootstock
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }


        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        /// <summary>
        /// abreviación.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }

    }
}