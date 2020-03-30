using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model.core;

namespace trifenix.agro.model.external.Input {
    public class SeasonInput : InputBase {

        [Required]
        public DateTime  StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool? Current { get; set; }

        [Required, ReferenceAttribute(typeof(CostCenter))]
        public string IdCostCenter { get; set; }

    }

    public class SeasonSwaggerInput {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? Current { get; set; }
        public string IdCostCenter { get; set; }
    }

}