using System.ComponentModel.DataAnnotations;
using trifenix.agro.model.external.Input;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro.model;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.model_input
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