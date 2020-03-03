using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input
{
    public class SpecieInput : InputBaseName
    {
        public string Abbreviation { get; set; }
    }

    public class SpecieSwaggerInput 
    {
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Abbreviation { get; set; }
    }




}
