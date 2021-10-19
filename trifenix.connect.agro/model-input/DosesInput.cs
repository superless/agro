using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;

using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de dosis
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.DOSES)]
    public class DosesInput : LocalBase
    {

        /// <summary>
        /// Búsqueda por referencia del id el producto asociado
        /// </summary>
        [Reference(typeof(Product))]
        [ReferenceSearch(EntityRelated.PRODUCT)]
        public string IdProduct { get; set; }

        /// <summary>
        /// Búsqueda por referencia de la variedad asociada
        /// </summary>
        [Reference(typeof(Variety))]
        [ReferenceSearch(EntityRelated.VARIETY)]
        public string[] IdVarieties { get; set; }

        /// <summary>
        /// Búsqueda por referencia de la especia asociada
        /// </summary>
        [Reference(typeof(Specie))]
        [ReferenceSearch(EntityRelated.SPECIE)]
        public string[] IdSpecies { get; set; }

        /// <summary>
        /// Búsqueda por referencia del objetivo de aplicación
        /// </summary>
        [Reference(typeof(ApplicationTarget))]
        [ReferenceSearch(EntityRelated.TARGET)]
        public string[] IdsApplicationTarget { get; set; }

        /// <summary>
        /// Horas para entrar al cuartel
        /// </summary>
        [Required]
        [Num32Search(NumRelated.HOURS_TO_ENTRY)]
        public int HoursToReEntryToBarrack { get; set; }

        /// <summary>
        /// Intervalo de días
        /// </summary>
        [Required]
        [Num32Search(NumRelated.DAYS_INTERVAL)]
        public int ApplicationDaysInterval { get; set; }

        /// <summary>
        /// Número de aplicaciones secuenciales
        /// </summary>
        [Required]
        [Num32Search(NumRelated.NUMBER_OF_SECQUENTIAL_APPLICATION)]
        public int NumberOfSequentialApplication { get; set; }

        /// <summary>
        /// !!!
        /// </summary>
        [Required]
        [Num32Search(NumRelated.WETTING_RECOMMENDED)]
        public int WettingRecommendedByHectares { get; set; }

        /// <summary>
        /// Búsqueda por referencia del tiempo esperado para cosecha(??? 
        /// </summary>
        [Required]
        [ReferenceSearch(EntityRelated.WAITINGHARVEST, true)]
        public WaitingHarvestInput[] WaitingToHarvest { get; set; }

        /// <summary>
        /// Cantidad mínima de dosis a aplicar
        /// </summary>
        [Required]
        [DoubleSearch(DoubleRelated.QUANTITY_MIN)]
        public double DosesQuantityMin { get; set; }

        /// <summary>
        /// Cantidad máxima de dosis
        /// </summary>
        [Required]
        [DoubleSearch(DoubleRelated.QUANTITY_MAX)]
        public double DosesQuantityMax { get; set; }

        /// <summary>
        /// Días a esperar
        /// </summary>
        [Num32Search(NumRelated.WAITING_DAYS)]
        public int? WaitingDaysLabel { get; set; }


        // <summary>
        /// determina si la dosis es por defecto.
        /// si un producto no se le asignan dosis, siempre tendrá uno.
        /// </summary>
        [Required]
        [BoolSearch(BoolRelated.GENERIC_DEFAULT)]
        public bool Default { get; set; }

        /// <summary>
        /// una dosis puede ser desactivada, si se requiere eliminar de un producto y esta está asociada con una orden.
        /// </summary>
        [Required]
        [BoolSearch(BoolRelated.GENERIC_ACTIVE)]
        public bool Active { get; set; }

        /// <summary>
        /// Id del cliente asociado con la compra
        /// </summary>
        public string ClientId { get; set; }


    }

    
}