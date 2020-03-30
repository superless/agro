using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {
    public class PreOrderInput : InputBase {

        [Required, Unique]
        public string Name { get; set; }

        [ReferenceAttribute(typeof(OrderFolder))]
        public string OrderFolderId { get; set; }

        [ReferenceAttribute(typeof(Ingredient))]
        public string IdIngredient { get; set; }

        [Required]
        public PreOrderType PreOrderType { get; set; }

        [Required, ReferenceAttribute(typeof(Barrack))]
        public string[] BarracksId { get; set; }

    }

    public class PreOrderSwaggerInput {

        [Required]
        public string Name { get; set; }

        public string OrderFolderId { get; set; }
        
        public string IdIngredient { get; set; }

        [Required]
        public PreOrderType PreOrderType { get; set; }

        [Required]
        public string[] BarracksId { get; set; }

    }

}