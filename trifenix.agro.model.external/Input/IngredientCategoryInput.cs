using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input {
    public class IngredientCategoryInput : InputBase {
        [Required, Unique]
        public string Name { get; set; }
    }


    public class IngredientCategorySwaggerInput {
        
        [Required]
        public string Name { get; set; }

    }

}