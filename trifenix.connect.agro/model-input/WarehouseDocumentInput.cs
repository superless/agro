using System;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.input;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Documento que monitorea el traspaso o salida de productos desde o hacia la bodega
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.WAREHOUSEDOCUMENT)]
    public class WarehouseDocumentInput : InputBase {


        [Required]
        [ReferenceSearch(EntityRelated.WAREHOUSE)]
        public string IdWarehouse { get; set; }

        /// <summary>
        /// Tipo de documento
        /// </summary>
        [Required]
        [EnumSearch(EnumRelated.DOCUMENT_TYPE)]
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// Fecha de emisión del documento
        /// </summary>
        [Required]
        [DateSearch(DateRelated.EMISSION_DATE)]
        public DateTime EmissionDate { get; set; }

        /// <summary>
        /// Tipo de pago del documento
        /// </summary>
        [Required]
        [EnumSearch(EnumRelated.PAYMENT_TYPE)]
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// Define si es un documento de entrada o salida
        /// </summary>
        [Required]
        [EnumSearch(EnumRelated.DOCUMENT_STATE)]
        public DocumentState DocumentState { get; set; }

        [Required]
        public bool Output { get; set; }

        /// <summary>
        /// Documento de producto sobre el cual realiza el documento de bodega
        /// </summary>
        [Required]
        [ReferenceSearch(EntityRelated.PRODUCTDOCUMENT, true)]
        public ProductDocumentInput[] ProductDocuments { get; set; }

        /// <summary>
        /// Búsqueda por referencia de la bodega asociada al documento
        /// </summary>
        [ReferenceSearch(EntityRelated.WAREHOUSEDESTINY)]
        public string Destiny { get; set; }


        /// <summary>
        /// Centro de costos de destino en caso de ser una transaccion de salida
        /// </summary>
        [ReferenceSearch(EntityRelated.COSTCENTER)]
        public string Source { get; set; }
    }
}