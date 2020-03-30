using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model;

namespace trifenix.agro.model.external.Input {
    public class IngredientInput : InputBase {

        [Required, Unique]
        public string Name { get; set; }

        [Required, Reference(typeof(IngredientCategory))]
        public string idCategory { get; set; }
    }

    public class IngredientSwaggerInput {

        [Required]
        public string Name { get; set; }

        [Required]
        public string IdCategory { get; set; }

    }

}