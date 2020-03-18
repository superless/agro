using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input {
    public class RootstockInput : InputBaseName {

        [Required, Unique]
        public string Abbreviation { get; set; }

    }

    public class RootstockSwaggerInput {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Abbreviation { get; set; }

    }

}