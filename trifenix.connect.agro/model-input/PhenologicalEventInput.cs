using System;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{

    [ReferenceSearchHeader(EntityRelated.PHENOLOGICAL_EVENT)]
    public class PhenologicalEventInput : InputBase {

        [Required, Unique]

        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        [DateSearch(DateRelated.START_DATE_PHENOLOGICAL_EVENT)]
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        [DateSearch(DateRelated.END_DATE_PHENOLOGICAL_EVENT)]
        public DateTime EndDate { get; set; }
    }

   

}