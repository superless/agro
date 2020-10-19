using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro.model_input;
using trifenix.connect.agro_model;
using trifenix.connect.input;
using trifenix.connect.mdm.validation_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{

    [ReferenceSearchHeader(EntityRelated.PRODUCT)]
    public class ProductInput : InputBase
    {

        [Required(ErrorMessage = "Nombre de producto es requerido")]
        [Unique]
        [SuggestSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        [Required, Reference(typeof(Ingredient))]
        [ReferenceSearch(EntityRelated.INGREDIENT)]
        public string IdActiveIngredient { get; set; }

        
        [ReferenceSearch(EntityRelated.BRAND)]
        [Required, Reference(typeof(Brand))]
        public string IdBrand { get; set; }

        public string ClientId { get; set; }


        [Required]
        [EnumSearch(EnumRelated.GENERIC_MEASURE_TYPE)]
        public MeasureType MeasureType { get; set; }


        [Unique]
        [SuggestSearch(StringRelated.SAG_CODE)]
        public string SagCode { get; set; }




        [ReferenceSearch(EntityRelated.DOSES)]
        public DosesInput[] Doses { get; set; }

    }


}