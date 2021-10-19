using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.validation_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de productos
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.PRODUCT)]
    public class ProductInput : LocalBase
    {
        /// <summary>
        /// Nombre 
        /// </summary>
        [Required(ErrorMessage = "Nombre de producto es requerido")]
        [Unique]
        [SuggestSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Búsqueda por referencia de ingrediente asociado
        /// </summary>
        [Required, Reference(typeof(Ingredient))]
        [ReferenceSearch(EntityRelated.INGREDIENT)]
        public string IdActiveIngredient { get; set; }

        /// <summary>
        /// Búsqueda por referencia de marca asociada
        /// </summary>
        [ReferenceSearch(EntityRelated.BRAND)]
        [Required, Reference(typeof(Brand))]
        public string IdBrand { get; set; }

        /// <summary>
        /// Id de cliente asociado a la compra del producto
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Tipo de medida
        /// </summary>
        [Required]
        [EnumSearch(EnumRelated.GENERIC_MEASURE_TYPE)]
        public MeasureType MeasureType { get; set; }

        /// <summary>
        /// Código Sag
        /// </summary>
        [Unique]
        [SuggestSearch(StringRelated.SAG_CODE)]
        public string SagCode { get; set; }

        /// <summary>
        /// Búsqueda por referencia de las dosis asociadas al producto
        /// </summary>
        [ReferenceSearch(EntityRelated.DOSES)]
        public DosesInput[] Doses { get; set; }

    }


}