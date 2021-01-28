using Cosmonaut.Attributes;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{

    /// <summary>
    /// Raíz de una planta.
    /// </summary>
    [SharedCosmosCollection("agro", "Rootstock")]
    [ReferenceSearchHeader(EntityRelated.ROOTSTOCK, PathName = "rootstock", Kind = EntityKind.ENTITY)]
    public class Rootstock : DocumentLocal {


        /// <summary>
        /// Identificador
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Nombre del rootstock
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }


        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        /// <summary>
        /// Abreviación.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }

    }
}