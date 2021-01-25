using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.input;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{

    [ReferenceSearchHeader(EntityRelated.TEST)]
    public class TestInput : InputBase
    {

        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        [Required, Unique]
        public string Abbreviation { get; set; }

        [StringSearch(StringRelated.GENERIC_NAME)]
        [Required, Unique]
        public string Name { get; set; }

        [StringSearch(StringRelated.GENERIC_BRAND)]
        [Required, Unique]
        public string Brand { get; set; }

    }
}