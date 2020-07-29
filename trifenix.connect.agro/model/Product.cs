using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.db;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model {

    /// <summary>
    /// Producto usado por las órdenes
    /// </summary>
    [SharedCosmosCollection("agro", "Product")]
    [ReferenceSearchHeader(EntityRelated.PRODUCT, Kind = EntityKind.CUSTOM_ENTITY, PathName = "products")]
    public class Product : DocumentBaseName<long>, ISharedCosmosEntity {


        /// <summary>
        /// Identificador
        /// </summary>
        public override string Id { get; set; }


        /// <summary>
        /// Nombre del producto.
        /// </summary>
        [SuggestSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        /// <summary>
        /// Identificador visual de producto
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }

        /// <summary>
        /// Ingrediente activo.
        /// </summary>
        [ReferenceSearch(EntityRelated.INGREDIENT)]
        public string IdActiveIngredient { get; set; }

        /// <summary>
        /// Marca.
        /// </summary>
        [StringSearch(StringRelated.GENERIC_BRAND)]
        public string Brand { get; set; }

        /// <summary>
        /// Tipo de medida
        /// </summary>
        [EnumSearch(EnumRelated.GENERIC_MEASURE_TYPE)]
        public MeasureType MeasureType { get; set; }

        /// <summary>
        /// Cantidad del producto por envase..
        /// </summary>
        [DoubleSearch(DoubleRelated.QUANTITY_CONTAINER)]
        public double Quantity { get; set; }

        /// <summary>
        /// tipo de envase.
        /// </summary>
        [EnumSearch(EnumRelated.GENERIC_KIND_CONTAINER)]
        public KindOfProductContainer KindOfBottle { get; set; }




    }

}