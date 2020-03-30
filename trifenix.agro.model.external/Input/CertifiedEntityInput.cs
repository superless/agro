using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input {
    public class CertifiedEntityInput : InputBase {
        [Required,Unique]
        public string Abbreviation { get; set; }

        [Required, Unique]
        public string Name { get; set; }
    }

    public class CertifiedEntitySwaggerInput {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Abbreviation { get; set; }
    }

}