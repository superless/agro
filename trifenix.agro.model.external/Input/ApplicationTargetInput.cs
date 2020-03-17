using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input {
    public class ApplicationTargetInput : InputBaseName {
        [Unique]
        public string Abbreviation { get; set; }
    }

    public class ApplicationTargetSwaggerInput {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Abbreviation { get; set; }
    }

}