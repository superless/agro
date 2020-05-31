using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

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