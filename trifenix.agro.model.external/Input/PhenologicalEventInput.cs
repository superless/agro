using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

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