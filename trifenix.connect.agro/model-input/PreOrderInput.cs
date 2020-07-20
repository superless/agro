using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.mdm_attributes;

namespace trifenix.agro.model.external.Input {

    [ReferenceSearchHeader(EntityRelated.PREORDER)]
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