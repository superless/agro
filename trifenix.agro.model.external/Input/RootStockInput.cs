using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input
{
    public class RootstockInput : InputBaseName
    {
        public string Abbreviation { get; set; }



    }

    public class RootstockSwaggerInput {


        [Required]
        public string Name { get; set; }


        [Required]
        public string Abbreviation { get; set; }
    }


}
