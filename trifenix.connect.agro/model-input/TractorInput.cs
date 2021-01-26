using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.input;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de tractores
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.TRACTOR)]
    public class TractorInput : InputBase {

        /// <summary>
        /// Marca
        /// </summary>
        [Required]
        [StringSearch(StringRelated.GENERIC_BRAND)]
        public string Brand { get; set; }

        /// <summary>
        /// Codigo
        /// </summary>
        [StringSearch(StringRelated.GENERIC_CODE)]
        [Required, Unique]
        public string Code { get; set; }

    }

}