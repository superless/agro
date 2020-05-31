using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {


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