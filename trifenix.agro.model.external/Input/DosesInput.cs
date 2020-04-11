using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.db.model;
using trifenix.agro.enums;
using trifenix.agro.enums.model;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

    [ReferenceSearch(EntityRelated.DOSES)]
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

    [ReferenceSearch(EntityRelated.WAITINGHARVEST, true)]
    public class WaitingHarvestInput {

        [DoubleSearch(DoubleRelated.PPM)]
        
        public double Ppm { get; set; }

        /// <summary>
        /// días de espera antes de la cosecha
        /// </summary>
        [Num32Search(NumRelated.WAITING_DAYS)]
        public int WaitingDays { get; set; }
        /// <summary>
        /// Entidad certificadora (opcional), si es indicado en la etiqueta, probablemente no sea de una entidad certificadora.
        /// </summary>
        [ReferenceSearch(EntityRelated.CERTIFIED_ENTITY)]
        public string IdCertifiedEntity { get; set; }


    }

    
}