using System.ComponentModel.DataAnnotations;
using trifenix.agro.model.external.Input;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{

    [ReferenceSearchHeader(EntityRelated.PLOTLAND)]
    public class PlotLandInput : InputBase {


        [Required, Reference(typeof(Sector))]
        [ReferenceSearch(EntityRelated.SECTOR)]
        public string IdSector { get; set; }

        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

    }

  

}