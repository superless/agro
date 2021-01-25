using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.input;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.validation_attributes;
using trifenix.connect.mdm_attributes;
using System;

namespace trifenix.connect.agro_model_input
{

    [ReferenceSearchHeader(EntityRelated.TEST)]
    public class TestInput : InputBase
    {

        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        [Required, Unique]
        public string Abbreviation { get; set; }

        [StringSearch(StringRelated.GENERIC_NAME)]
        [Required, Unique]
        public string Name { get; set; }

        [ReferenceSearch(EntityRelated.BRAND)]
        [Required, Reference(typeof(Brand))]
        public string IdBrand { get; set; }

        [Required]
        [DateSearch(DateRelated.START_DATE_SEASON)]
        public DateTime StartDate { get; set; }

        [Required]
        [DateSearch(DateRelated.END_DATE_SEASON)]
        public DateTime EndDate { get; set; }

        [Required]
        [BoolSearch(BoolRelated.CURRENT)]
        public bool? Activo { get; set; }

    }
}