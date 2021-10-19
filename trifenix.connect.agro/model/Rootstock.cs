using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;
using trifenix.connect.model;

namespace trifenix.connect.agro_model
{

    /// <summary>
    /// Raíz de una planta.
    /// </summary>    
    [ReferenceSearchHeader(EntityRelated.ROOTSTOCK, PathName = "rootstock", Kind = EntityKind.ENTITY)]
    [GroupMenu("Principales", PhisicalDevice.ALL, "Especies")]
    public class Rootstock : DocumentDb {


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