using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{

    /// <summary>
    /// Ingreso de nombre de negocio
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.BUSINESSNAME)]
    public class BusinessNameInput : LocalBase {

        /// <summary>
        /// Nombre
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        [Required, Unique]
        public string Name { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [StringSearch(StringRelated.GENERIC_EMAIL)]
        [Required, Unique]
        public string Email { get; set; }

        /// <summary>
        /// RUT
        /// </summary>
        [StringSearch(StringRelated.GENERIC_RUT)]
        [Required, Unique]
        public string Rut { get; set; }

        /// <summary>
        /// WebPage
        /// </summary>
        [StringSearch(StringRelated.GENERIC_WEBPAGE)]
        public string WebPage { get; set; }

        /// <summary>
        /// Giro monetario
        /// </summary>
        [StringSearch(StringRelated.GENERIC_GIRO)]
        public string Giro { get; set; }

        /// <summary>
        /// Número de teléfono
        /// </summary>
        [StringSearch(StringRelated.GENERIC_PHONE)]
        public string Phone { get; set; }
    }

   

}