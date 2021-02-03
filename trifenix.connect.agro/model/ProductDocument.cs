using Cosmonaut.Attributes;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.model
{
    /// <summary>
    /// Documento de movimiento de productos desde y hacia bodega
    /// </summary>
    [SharedCosmosCollection("agro", "ProductDocument")]
    [ReferenceSearchHeader(EntityRelated.PRODUCTDOCUMENT, PathName = "ProductDocument", Kind = EntityKind.CUSTOM_ENTITY)]
    public class ProductDocument : DocumentLocal
    {
        /// <summary>
        /// Identificador.
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Autonumérico del identificador del cliente.
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }

        /// <summary>
        /// Búsqueda por referencia del producto asociado al documento
        /// </summary>
        [ReferenceSearch(EntityRelated.PRODUCT)]
        public string IdProduct { get; set; }

        /// <summary>
        /// Cantidad de productos 
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Precio de los productos
        /// </summary>
        public int Price { get; set; }
    }
 }