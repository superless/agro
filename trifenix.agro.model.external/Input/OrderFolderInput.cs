using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input
{
    public class OrderFolderInput : InputBase {

        public string IdPhenologicalEvent { get; set; }
        public string IdApplicationTarget { get; set; }
        public string IdSpecie { get; set; }
        
        public string IdIngredient { get; set; }

        public string IdCategoryIngredient { get; set; }

    }

    public class OrderFolderSwaggerInput
    {

        [Required]
        public string IdPhenologicalEvent { get; set; }
        [Required]
        public string IdApplicationTarget { get; set; }

        [Required]
        public string IdSpecie { get; set; }

        public string IdIngredient { get; set; }

        public string IdCategoryIngredient { get; set; }

    }
}
