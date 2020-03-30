using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

    [ReferenceSearch(EntityRelated.SECTOR)]
    public class SectorInput : InputBase {
        [Required, Unique]


        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }
    }

   
    
}