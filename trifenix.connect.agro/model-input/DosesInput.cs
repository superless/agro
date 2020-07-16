using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.model.external.Input;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro.model;

namespace trifenix.connect.agro.model_input
{

    [ReferenceSearchHeader(EntityRelated.DOSES)]
    public class DosesInput : InputBase {

        [Required, Reference(typeof(Product))]
        [ReferenceSearch(EntityRelated.PRODUCT)]
        public string IdProduct { get; set; }

        [Reference(typeof(Variety))]
        [ReferenceSearch(EntityRelated.VARIETY)]
        public string[] IdVarieties { get; set; }

        [Reference(typeof(Specie))]
        [ReferenceSearch(EntityRelated.SPECIE)]
        public string[] IdSpecies { get; set; }

        [Reference(typeof(ApplicationTarget))]
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

        [Required]
        [EnumSearch(EnumRelated.DOSES_APPLICATED_TO)]
        public DosesApplicatedTo DosesApplicatedTo { get; set; }

        [JsonIgnore]
        public bool Active { get; set; }

        [JsonIgnore]
        public bool Default { get; set; }

    }

    
}