using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input {

    public class ApplicationTargetInput : InputBase {

        [Required, Unique]
        public string Abbreviation { get; set; }

        [Required, Unique]
        public string Name { get; set; }

    }

    public class TargetSwaggerInput {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Abbreviation { get; set; }

    }

}