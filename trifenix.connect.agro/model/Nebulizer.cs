using Cosmonaut.Attributes;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Entidad encargada de generar los nebulizadores.
    /// Un nebulizador es el tractor encargado de el riego.
    /// </summary>
    [SharedCosmosCollection("agro", "Nebulizer")]
    [ReferenceSearchHeader(EntityRelated.NEBULIZER, PathName ="nebulizers", Kind = EntityKind.ENTITY)]
    [GroupMenu(MenuEntityRelated.MANTENEDORES, PhisicalDevice.ALL, SubMenuEntityRelated.MAQUINARIA)]
    public class Nebulizer : DocumentLocal {

        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual de la entidad certificadora
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }


        /// <summary>
        /// Marca del tractor
        /// </summary>
        [StringSearch(StringRelated.GENERIC_BRAND)]
        public string Brand { get; set; }


        /// <summary>
        /// Código de la nebulizadora.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_CODE)]
        public string Code { get; set; }

    }
}