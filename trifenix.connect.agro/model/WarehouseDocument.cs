using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.model
{
    /// <summary>
    /// Documento de movimiento de productos desde y hacia bodega
    /// </summary>
    [SharedCosmosCollection("agro", "WarehouseDocument")]
    [ReferenceSearchHeader(EntityRelated.WAREHOUSEDOCUMENT, PathName = "WarehouseDocument", Kind = EntityKind.CUSTOM_ENTITY)]
    public class WarehouseDocument : DocumentLocal
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


        [ReferenceSearch(EntityRelated.WAREHOUSE)]
        public string IdWarehouse { get; set; }

        /// <summary>
        /// Tipo de documento
        /// </summary>
        [EnumSearch(EnumRelated.DOCUMENT_TYPE)]
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// Fecha de emisión del documento
        /// </summary>
        [DateSearch(DateRelated.EMISSION_DATE)]
        public DateTime EmissionDate { get; set; }
    
        /// <summary>
        /// Tipo de pago
        /// </summary>
        [EnumSearch(EnumRelated.PAYMENT_TYPE)]
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// Determina el estado del documento 
        /// </summary>
        [EnumSearch(EnumRelated.DOCUMENT_STATE)]
        public DocumentState DocumentState { get; set; }

        /// <summary>
        /// Determina si el documento es de entrada o salida
        /// </summary>
        public bool Output { get; set; }

        private List<ProductDocument> _productsDocument;

        /// <summary>
        /// Dias para cosechar por entidad certificadora
        /// </summary>
        [ReferenceSearch(EntityRelated.PRODUCTDOCUMENT, true)]
        public List<ProductDocument> ProductDocuments
        {
            get
            {
                _productsDocument = _productsDocument ?? new List<ProductDocument>();
                return _productsDocument;
            }
            set { _productsDocument = value; }
        }

        /// <summary>
        /// Búsqueda por referencia de la bodega asociada
        /// </summary>
        [ReferenceSearch(EntityRelated.WAREHOUSEDESTINY)]
        public string Destiny { get; set; }

        /// <summary>
        /// Búsqueda por referencia de la empresa asociada
        /// </summary>
        [ReferenceSearch(EntityRelated.BUSINESSNAME)]
        public string Source { get; set; }
    }
}
