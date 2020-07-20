using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{

    [ReferenceSearchHeader(EntityRelated.TRACTOR)]
    public class TractorInput : InputBase {

        [Required]
        [StringSearch(StringRelated.GENERIC_BRAND)]
        public string Brand { get; set; }

        [StringSearch(StringRelated.GENERIC_CODE)]
        [Required, Unique]
        public string Code { get; set; }

    }

}