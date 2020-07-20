using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{

    [ReferenceSearchHeader(EntityRelated.SECTOR)]
    public class SectorInput : InputBase {
        [Required, Unique]


        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }
    }

   
    
}