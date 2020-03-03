using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input
{
    public class IngredientInput : InputBaseName
    {
        
        public string idCategory { get; set; }
    }

    public class IngredientSwaggerInput {


        [Required]
        public string Name { get; set; }

        [Required]
        public string IdCategory { get; set; }
    }

}
