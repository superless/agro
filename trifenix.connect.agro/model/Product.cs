
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;
using trifenix.connect.model;

namespace trifenix.connect.agro_model
{

    /// <summary>
    /// Producto usado por las órdenes
    /// </summary>
    
    [ReferenceSearchHeader(EntityRelated.PRODUCT, Kind = EntityKind.CUSTOM_ENTITY, PathName = "products")]
    [GroupMenu("Complementarios", PhisicalDevice.ALL, "Productos")]
    public class Product : DocumentDb
    {

        /// <summary>
        /// Identificador
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Nombre del producto.
        /// </summary>
        [SuggestSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Identificador visual de producto
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        /// <summary>
        /// Ingrediente activo.
        /// </summary>
        [ReferenceSearch(EntityRelated.INGREDIENT)]
        public string IdActiveIngredient { get; set; }

        /// <summary>
        /// Marca.
        /// </summary>
        [ReferenceSearch(EntityRelated.BRAND)]
        public string IdBrand { get; set; }

        /// <summary>
        /// Tipo de medida
        /// </summary>
        [EnumSearch(EnumRelated.GENERIC_MEASURE_TYPE)]
        public MeasureType MeasureType { get; set; }

        [SuggestSearch(StringRelated.SAG_CODE)]
        public string SagCode { get; set; }
    }



}