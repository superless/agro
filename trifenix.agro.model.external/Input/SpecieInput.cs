using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input {
    public class SpecieInput : InputBase {

        [Required, Unique]
        public string Name { get; set; }

        [Required, UniqueAttribute]
        public string Abbreviation { get; set; }
    }

    public class SpecieSwaggerInput {
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Abbreviation { get; set; }
    }

}