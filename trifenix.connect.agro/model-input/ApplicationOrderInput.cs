using System;
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
    /// Input de orden de aplicación
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.ORDER)]
    public class ApplicationOrderInput : LocalBase {

        /// <summary>
        /// Nombre de la orden
        /// </summary>
        [Required, Unique]
        [SuggestSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Tipo de la orden
        /// </summary>
        [Required]
        [EnumSearch(EnumRelated.ORDER_TYPE)]
        public OrderType OrderType { get; set; }

        /// <summary>
        /// Fecha de inicio
        /// </summary>
        [Required]
        [DateSearch(DateRelated.START_DATE_ORDER)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Fecha de termino
        /// </summary>
        [Required]
        [DateSearch(DateRelated.END_DATE_ORDER)]
        public DateTime EndDate { get; set; }


        [Required]
        [DoubleSearch(DoubleRelated.WETTING)]
        public double Wetting { get; set; }

        /// <summary>
        /// Busca por referencia una orden de dosis
        /// </summary>
        [Required]
        [ReferenceSearch(EntityRelated.DOSES_ORDER, true)]
        public DosesOrder[] DosesOrder { get; set; }

        /// <summary>
        /// Busca por referencia una preorden
        /// </summary>
        [Reference(typeof(PreOrder))]
        [ReferenceSearch(EntityRelated.PREORDER)]
        public string[] IdsPreOrder { get; set; }

        /// <summary>
        /// Busca por referencia una instancia de barrack
        /// </summary>
        [Required]
        [ReferenceSearch(EntityRelated.BARRACK_EVENT, true)]
        public BarrackOrderInstance[] Barracks { get; set; }

    }


   

}