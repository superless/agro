using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

    [ReferenceSearch(EntityRelated.PHENOLOGICAL_EVENT)]
    public class PhenologicalEventInput : InputBase {

        [Required, Unique]

        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        [DateSearch(DateRelated.START_DATE)]
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        [DateSearch(DateRelated.END_DATE)]
        public DateTime EndDate { get; set; }
    }

   

}