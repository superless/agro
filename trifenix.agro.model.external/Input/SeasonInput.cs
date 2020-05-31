using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.db.model.core;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

    [ReferenceSearchHeader(EntityRelated.SEASON)]
    public class SeasonInput : InputBase {

        [Required]
        [DateSearch(DateRelated.START_DATE_SEASON)]
        public DateTime  StartDate { get; set; }

        [Required]
        [DateSearch(DateRelated.END_DATE_SEASON)]
        public DateTime EndDate { get; set; }

        [BoolSearch(BoolRelated.CURRENT)]
        public bool? Current { get; set; }

        [Required, Reference(typeof(CostCenter))]
        [ReferenceSearch(EntityRelated.COSTCENTER)]
        public string IdCostCenter { get; set; }

    }

   
}