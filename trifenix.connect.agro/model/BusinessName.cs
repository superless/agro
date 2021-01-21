using Cosmonaut.Attributes;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model
{

    /// <summary>
    /// Razón social de cada uno de los centro de costos.
    /// </summary>
    [SharedCosmosCollection("agro", "BusinessName")]
    [ReferenceSearchHeader(EntityRelated.BUSINESSNAME, PathName = "business_names", Kind =EntityKind.ENTITY)]
    [GroupMenu(MenuEntityRelated.MANTENEDORES, PhisicalDevice.ALL, SubMenuEntityRelated.GESTION)]
    public class BusinessName : DocumentLocal {
        

        /// <summary>
        /// Identificador de la razón social.
        /// </summary>
        public override string Id { get; set; }


        /// <summary>
        /// Autonumérico del identificador del cliente.
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }



        /// <summary>
        /// Nombre de la razón social.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        [Group(0,PhisicalDevice.WEB, 6)]
        [Required]
        public string Name { get; set; }


        /// <summary>
        /// Correo electrónico de la razón social.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_EMAIL)]
        [Group(0, PhisicalDevice.WEB, 6)]
        [Required]
        public string Email { get; set; }


        /// <summary>
        /// Rut de la razón social.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_RUT)]
        [Group(1, PhisicalDevice.WEB, 3)]
        [Required]
        public string Rut { get; set; }


        /// <summary>
        /// Página web de la razón social.
        /// </summary>
        [Group(1, PhisicalDevice.WEB, 3)]
        [StringSearch(StringRelated.GENERIC_WEBPAGE)]
        public string WebPage { get; set; }


        /// <summary>
        /// Giro de la razón social.
        /// </summary>
        [Group(1, PhisicalDevice.WEB, 3)]
        [StringSearch(StringRelated.GENERIC_GIRO)]
        public string Giro { get; set; }


        /// <summary>
        /// Teléfono
        /// </summary>
        [Group(1, PhisicalDevice.WEB, 3)]
        [StringSearch(StringRelated.GENERIC_PHONE)]
        public string Phone { get; set; }

    }

}