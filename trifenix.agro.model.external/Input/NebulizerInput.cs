using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

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