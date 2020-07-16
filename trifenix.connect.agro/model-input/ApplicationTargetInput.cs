using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.model_input
{

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