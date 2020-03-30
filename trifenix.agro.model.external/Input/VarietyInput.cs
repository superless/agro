using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model;

namespace trifenix.agro.model.external.Input {
    public class VarietyInput : InputBase {

        [Required, Unique]
        public string Name { get; set; }

        [Required, Unique]
        public string Abbreviation { get; set; }

        [Required, Reference(typeof(Specie))]
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