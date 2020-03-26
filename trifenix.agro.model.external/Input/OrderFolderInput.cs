using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.model.external.Input {
    public class OrderFolderInput : InputBase {

        [Required, Reference(typeof(PhenologicalEvent))]
        public string IdPhenologicalEvent { get; set; }

        [Required, Reference(typeof(ApplicationTarget))]
        public string IdApplicationTarget { get; set; }

        [Required, Reference(typeof(Specie))]
        public string IdSpecie { get; set; }

        [Reference(typeof(Ingredient))]
        public string IdIngredient { get; set; }

        [Required, Reference(typeof(IngredientCategory))]
        public string IdIngredientCategory { get; set; }

    }

    public class OrderFolderSwaggerInput {

        [Required]
        public string IdPhenologicalEvent { get; set; }
        [Required]
        public string IdApplicationTarget { get; set; }

        [Required]
        public string IdSpecie { get; set; }

        public string IdIngredient { get; set; }

        public string IdIngredientCategory { get; set; }

    }

}