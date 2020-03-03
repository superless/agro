using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input
{
    public class RootStockInput : InputBaseName
    {
        public string Abbreviation { get; set; }



    }

    public class RootStockSwaggerInput {


        [Required]
        public string Name { get; set; }


        [Required]
        public string Abbreviation { get; set; }
    }


}
