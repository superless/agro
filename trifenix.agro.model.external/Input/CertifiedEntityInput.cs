using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input
{
    public class CertifiedEntityInput : InputBaseName
    {
        

        public string Abbreviation { get; set; }
    }

    public class CertifiedEntitySwaggerInput 
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Abbreviation { get; set; }
    }



}
