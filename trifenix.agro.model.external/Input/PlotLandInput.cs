using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.db.model;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

    [ReferenceSearch(EntityRelated.PLOTLAND)]
    public class PlotLandInput : InputBase {


        [Required, Reference(typeof(Sector))]
        [ReferenceSearch(EntityRelated.SECTOR)]
        public string IdSector { get; set; }

        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

    }

  

}