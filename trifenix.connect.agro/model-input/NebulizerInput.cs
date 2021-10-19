using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de nebulizador
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.NEBULIZER)]
    public class NebulizerInput : LocalBase
    {

        /// <summary>
        /// Nombre
        /// </summary>
        [Required]
        [StringSearch(StringRelated.GENERIC_BRAND)]
        public string Brand { get; set; }

        /// <summary>
        /// Codigo
        /// </summary>
        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_CODE)]
        public string Code { get; set; }

    }

   

}