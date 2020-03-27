using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.model.external.Input {
    public class VarietyInput : InputBaseName {
        
        [Required, UniqueAttribute]
        public string Abbreviation { get; set; }

        [Required, ReferenceAttribute(typeof(Specie))]
        public string IdSpecie { get; set; }

    }

    public class VarietySwaggerInput {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Abbreviation { get; set; }

        [Required]
        public string IdSpecie { get; set; }

    }

}