using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.db.model;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

    [ReferenceSearch(EntityRelated.PREORDER)]
    public class PreOrderInput : InputBase {

        [Required, Unique]
        [SuggestSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        [Reference(typeof(OrderFolder))]
        [ReferenceSearch(EntityRelated.ORDER_FOLDER)]
        public string OrderFolderId { get; set; }

        [Reference(typeof(Ingredient))]
        public string IdIngredient { get; set; }

        [Required]
        [EnumSearch(EnumRelated.PRE_ORDER_TYPE)]
        public PreOrderType PreOrderType { get; set; }

        [Required, Reference(typeof(Barrack))]
        [ReferenceSearch(EntityRelated.BARRACK)]
        public string[] BarracksId { get; set; }

    }

    

}