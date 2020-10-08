using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.input;
using trifenix.connect.mdm.validation_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{

    [ReferenceSearchHeader(EntityRelated.COSTCENTER)]
    public class CostCenterInput : InputBase {

        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }
        [Required,Reference(typeof(BusinessName))]
        [ReferenceSearch(EntityRelated.BUSINESSNAME)]
        public string IdBusinessName { get; set; }

    }

  

}