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
    /// Determina cual es el objetivo de la aplicación de una órden.
    /// </summary>
    [SharedCosmosCollection("agro", "ApplicationTarget")]
    [ReferenceSearchHeader(EntityRelated.TARGET, PathName = "targets", Kind = EntityKind.ENTITY)]
    [GroupMenu(MenuEntityRelated.MANTENEDORES, PhisicalDevice.ALL, SubMenuEntityRelated.ORDEN_APLICACION)]
    public class ApplicationTarget : DocumentLocal {


        /// <summary>
        /// Identificador
        /// </summary>
        public override string Id { get; set; }


        /// <summary>
        /// El identificador de cliente que será mostrado en el formulario y la vista.
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }


        /// <summary>
        /// Nombre del objetivo de la aplicación
        /// </summary>
        [Group(0, PhisicalDevice.WEB, proportion: 6)]
        [Required]
        [Unique]

        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Abreviación del objetivo.
        /// </summary>
        [Group(0, PhisicalDevice.WEB, proportion:3)]
        [Unique]        
        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }

    }

}