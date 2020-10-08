using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.input;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{

    [ReferenceSearchHeader(EntityRelated.NEBULIZER)]
    public class NebulizerInput : InputBase {

        [Required]
        [StringSearch(StringRelated.GENERIC_BRAND)]
        public string Brand { get; set; }

        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_CODE)]
        public string Code { get; set; }

    }

   

}