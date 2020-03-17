using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input {
    public class IngredientCategoryInput : InputBaseName { }

    public class IngredientCategorySwaggerInput {
        
        [Required]
        public string Name { get; set; }

    }

}