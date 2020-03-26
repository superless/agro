using System;
using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input {
    public class PhenologicalEventInput : InputBaseName {
        
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }

    public class PhenologicalEventSwaggerInput {

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }

}