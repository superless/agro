using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums.model;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model {

    /// <summary>
    /// Producto Quimico usado por las órdenes
    /// </summary>
    [SharedCosmosCollection("agro", "Product")]
    [ReferenceSearch(EntityRelated.PRODUCT)]
    public class Product : DocumentBaseName, ISharedCosmosEntity {

        public override string Id { get; set; }

        [SuggestSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [ReferenceSearch(EntityRelated.INGREDIENT)]
        public string IdActiveIngredient { get; set; }

        [StringSearch(StringRelated.GENERIC_BRAND)]
        public string Brand { get; set; }

        [EnumSearch(EnumRelated.GENERIC_MEASURE_TYPE)]
        public MeasureType MeasureType { get; set; }

        [DoubleSearch(DoubleRelated.QUANTITY_CONTAINER)]
        public double Quantity { get; set; }

        [EnumSearch(EnumRelated.GENERIC_KIND_CONTAINER)]
        public KindOfProductContainer KindOfBottle { get; set; }

    }

}