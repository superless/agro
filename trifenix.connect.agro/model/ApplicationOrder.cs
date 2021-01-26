using Cosmonaut.Attributes;
using System;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Entidad encargada de la orden de aplicación
    /// </summary>
    [SharedCosmosCollection("agro", "ApplicationOrder")]
    [ReferenceSearchHeader(EntityRelated.ORDER, Kind = EntityKind.PROCESS, PathName = "orders")]
    public class ApplicationOrder : DocumentLocal {
        /// <summary>
        /// Identificador de la entidad
        /// </summary>
        public override string Id { get; set; }


        /// <summary>
        /// Identificador autonumérico.
        /// </summary>
        [AutoNumericSearch(StringRelated.GENERIC_CORRELATIVE)]
        public override string ClientId { get; set; }


        /// <summary>
        /// Tipo de Orden.
        /// </summary>
        [EnumSearch(EnumRelated.ORDER_TYPE)]
        public OrderType OrderType { get; set; }


        /// <summary>
        /// Nombre de la orden
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }


        /// <summary>
        /// Fecha de inicio de la orden
        /// </summary>
        [DateSearch(DateRelated.START_DATE_ORDER)]
        public DateTime StartDate { get; set; }


        /// <summary>
        /// Fecha fin de la orden
        /// </summary>
        [DateSearch(DateRelated.END_DATE_ORDER)]
        public DateTime EndDate { get; set; }


        /// <summary>
        /// Mojamiento asignado a la orden
        /// </summary>
        [DoubleSearch(DoubleRelated.WETTING)]
        public double Wetting { get; set; }


        /// <summary>
        /// Dosis aplicadas (producto).
        /// </summary>
        [ReferenceSearch(EntityRelated.DOSES_ORDER, true)]
        public DosesOrder[] DosesOrder { get; set; }


        /// <summary>
        /// Identificador de preordenes relacionadas con la orden.
        /// </summary>
        [ReferenceSearch(EntityRelated.PREORDER)]
        public string[] IdsPreOrder { get; set; }


        /// <summary>
        /// Cuarteles relacionados.
        /// </summary>
        [ReferenceSearch(EntityRelated.BARRACK_EVENT, true)]
        public BarrackOrderInstance[] Barracks { get; set; }

    }

    /// <summary>
    /// Asocia cuarteles a una orden y las notificaciones desde 
    /// monitoreo.
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.BARRACK_EVENT, true)]
    public class BarrackOrderInstance {


        /// <summary>
        /// Identificador de un cuartel.
        /// </summary>
        [ReferenceSearch(EntityRelated.BARRACK)]
        public string  IdBarrack { get; set; }


        /// <summary>
        /// Identificadores de las notificaciones asociadas.
        /// </summary>
        [ReferenceSearch(EntityRelated.NOTIFICATION_EVENT)]
        public string[] IdNotificationEvents { get; set; }

    }


    /// <summary>
    /// Asocia las dosis a la orden, 
    /// la dosis viene asociada con el producto.
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.DOSES_ORDER, true)]
    public class DosesOrder {


        /// <summary>
        /// Identificador de la dosis aplicada.
        /// La dosis incluye todos los datos del etiquetado.
        /// </summary>
        [ReferenceSearch(EntityRelated.DOSES)]
        public string IdDoses { get; set; }


        /// <summary>
        /// Cantidad de producto que se aplicará por hectarea.
        /// </summary>
        [DoubleSearch(DoubleRelated.QUANTITY_APPLIED)]
        public double QuantityByHectare { get; set; }

    }

}