using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.input;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{

    [ReferenceSearchHeader(EntityRelated.DOSES)]
    public class DosesInput : InputBase {

        [Reference(typeof(Product))]
        [ReferenceSearch(EntityRelated.PRODUCT)]
        public string IdProduct { get; set; }

        [Reference(typeof(Variety))]
        [ReferenceSearch(EntityRelated.VARIETY)]
        public string[] IdVarieties { get; set; }

        [Reference(typeof(Specie))]
        [ReferenceSearch(EntityRelated.SPECIE)]
        public string[] IdSpecies { get; set; }

        [Reference(typeof(ApplicationTarget))]
        [ReferenceSearch(EntityRelated.TARGET)]
        public string[] IdsApplicationTarget { get; set; }

        [Required]
        [Num32Search(NumRelated.HOURS_TO_ENTRY)]
        public int HoursToReEntryToBarrack { get; set; }

        [Required]
        [Num32Search(NumRelated.DAYS_INTERVAL)]
        public int ApplicationDaysInterval { get; set; }

        [Required]
        [Num32Search(NumRelated.NUMBER_OF_SECQUENTIAL_APPLICATION)]
        public int NumberOfSequentialApplication { get; set; }

        [Required]
        [Num32Search(NumRelated.WETTING_RECOMMENDED)]
        public int WettingRecommendedByHectares { get; set; }

        [Required]
        [ReferenceSearch(EntityRelated.WAITINGHARVEST, true)]
        public WaitingHarvestInput[] WaitingToHarvest { get; set; }

        [Required]
        [DoubleSearch(DoubleRelated.QUANTITY_MIN)]
        public double DosesQuantityMin { get; set; }

        [Required]
        [DoubleSearch(DoubleRelated.QUANTITY_MAX)]
        public double DosesQuantityMax { get; set; }

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



        public string ClientId { get; set; }


    }

    
}