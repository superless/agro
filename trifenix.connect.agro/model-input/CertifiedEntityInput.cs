using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.model_input
{

    [ReferenceSearchHeader(EntityRelated.CERTIFIED_ENTITY)]
    public class CertifiedEntityInput : InputBase {
        [Required,Unique]
        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }

        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }
    }

    

}