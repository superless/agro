using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

    [ReferenceSearchHeader(EntityRelated.TARGET)]
    public class ApplicationTargetInput : InputBase {


        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        [Required, Unique]
        public string Abbreviation { get; set; }

        [StringSearch(StringRelated.GENERIC_NAME)]
        [Required, Unique]
        public string Name { get; set; }

    }

   

}