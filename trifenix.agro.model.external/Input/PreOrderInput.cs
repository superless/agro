using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {
    public class PreOrderInput : InputBaseName {
        
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