using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Documento que monitorea el traspaso o salida de productos desde o hacia la bodega
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.PRODUCTDOCUMENT)]
    public class ProductDocumentInput {

        /// <summary>
        /// Id del producto relacionado
        /// </summary>
        [ReferenceSearch(EntityRelated.PRODUCT)]
        [Required, Reference(typeof(Product))]
        public string IdProduct { get; set; }

        /// <summary>
        /// Cantidad de productos
        /// </summary>
        [Required]
        [Num32Search(NumRelated.QUANTITY)]
        public int Quantity { get; set; }

        /// <summary>
        /// Precio total de los productos con ese id
        /// </summary>
        [Required]
        [Num32Search(NumRelated.PRICE)]
        public int Price { get; set; }
    }
}
