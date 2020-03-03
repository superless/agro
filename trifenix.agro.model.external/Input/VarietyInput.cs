using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input
{
    public class VarietyInput : InputBaseName
    {
        

        public string Abbreviation { get; set; }

        public string IdSpecie { get; set; }



    }

    public class VarietySwaggerInput : InputBaseName
    {


        [Required]
        public string Name { get; set; }

        [Required]
        public string Abbreviation { get; set; }

        [Required]
        public string IdSpecie { get; set; }



    }
}
