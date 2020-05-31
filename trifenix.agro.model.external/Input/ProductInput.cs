using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.db.model;
using trifenix.agro.enums;
using trifenix.agro.enums.model;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

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