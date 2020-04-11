using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.db.model;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

    [ReferenceSearch(EntityRelated.INGREDIENT)]
    public class IngredientInput : InputBase {

        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        [Required, Reference(typeof(IngredientCategory))]
        [ReferenceSearch(EntityRelated.CATEGORY_INGREDIENT)]
        public string idCategory { get; set; }
    }

   

}