using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.model.external.Input {
    public class IngredientInput : InputBaseName {
        
        [Required, ReferenceAttribute(typeof(IngredientCategory))]
        public string idCategory { get; set; }
    }

    public class IngredientSwaggerInput {

        [Required]
        public string Name { get; set; }

        [Required]
        public string IdCategory { get; set; }

    }

}