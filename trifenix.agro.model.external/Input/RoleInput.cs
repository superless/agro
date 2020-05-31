using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

    [ReferenceSearchHeader(EntityRelated.ROLE)]
    public class RoleInput : InputBase {

        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

    }

}