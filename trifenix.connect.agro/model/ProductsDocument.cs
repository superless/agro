using Cosmonaut.Attributes;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro.model
{
    /// <summary>
    /// Documento de movimiento de productos desde y hacia bodega
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.PRODUCTSDOCUMENT, true, Kind = EntityKind.CUSTOM_ENTITY)]
    public class ProductsDocument : DocumentLocal
    {

        /// <summary>
        /// Identificador
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual de la bodega
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        /// <summary>
        /// Búsqueda por referencia del producto asociado al documento
        /// </summary>
        [ReferenceSearch(EntityRelated.PRODUCT)]
        [Reference(typeof(Product))]
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