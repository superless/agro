using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.input;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{


    [ReferenceSearchHeader(EntityRelated.BUSINESSNAME)]
    public class BusinessNameInput : InputBase {

        [StringSearch(StringRelated.GENERIC_NAME)]
        [Required, Unique]
        public string Name { get; set; }

        [StringSearch(StringRelated.GENERIC_EMAIL)]
        [Required, Unique]
        public string Email { get; set; }

        [StringSearch(StringRelated.GENERIC_RUT)]
        [Required, Unique]
        public string Rut { get; set; }

        [StringSearch(StringRelated.GENERIC_WEBPAGE)]
        public string WebPage { get; set; }

        [StringSearch(StringRelated.GENERIC_GIRO)]
        public string Giro { get; set; }

        [StringSearch(StringRelated.GENERIC_PHONE)]
        public string Phone { get; set; }
    }

   

}