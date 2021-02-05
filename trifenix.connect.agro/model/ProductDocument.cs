using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.model
{
    /// <summary>
    /// Documento de movimiento de productos desde y hacia bodega
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.PRODUCTDOCUMENT, true, Kind = EntityKind.CUSTOM_ENTITY)]
    public class ProductDocument {

        /// <summary>
        /// Búsqueda por referencia del producto asociado al documento
        /// </summary>
        [ReferenceSearch(EntityRelated.PRODUCT)]
        public string IdProduct { get; set; }

        /// <summary>
        /// Cantidad de productos 
        /// </summary>
        [Num32Search(NumRelated.QUANTITY)]
        public int Quantity { get; set; }

        /// <summary>
        /// Precio de los productos
        /// </summary>
        [Num32Search(NumRelated.PRICE)]
        public int Price { get; set; }

    }
}