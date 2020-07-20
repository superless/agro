using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.model.external.Input;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;

namespace trifenix.connect.agro_model_input
{

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