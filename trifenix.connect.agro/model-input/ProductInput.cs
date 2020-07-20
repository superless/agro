using System.ComponentModel.DataAnnotations;
using trifenix.agro.model.external.Input;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{

    [ReferenceSearchHeader(EntityRelated.PRODUCT)]
    public class ProductInput : InputBase {

        [Required, Unique]
        [SuggestSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        [Required, Reference(typeof(Ingredient))]
        [ReferenceSearch(EntityRelated.INGREDIENT)]
        public string IdActiveIngredient { get; set; }

        [Required]
        [StringSearch(StringRelated.GENERIC_BRAND)]
        public string Brand { get; set; }

        [Required]
        [EnumSearch(EnumRelated.GENERIC_MEASURE_TYPE)]
        public MeasureType MeasureType { get; set; }

        [Required]
        [DoubleSearch(DoubleRelated.QUANTITY_CONTAINER)]
        public double Quantity { get; set; }

        [Required]
        [EnumSearch(EnumRelated.GENERIC_KIND_CONTAINER)]
        public KindOfProductContainer KindOfBottle { get; set; }

        
        [ReferenceSearch(EntityRelated.DOSES)]
        public DosesInput[] Doses { get; set; }

    }


}