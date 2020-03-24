using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;

namespace trifenix.agro.model.external.Input {
    public class DosesInput : InputBase {

        [Required, Reference(typeof(Product))]
        public string IdProduct { get; set; }

        [Reference(typeof(Variety))]
        public string[] IdVarieties { get; set; }

        [Reference(typeof(Specie))]
        public string[] IdSpecies { get; set; }

        [Reference(typeof(ApplicationTarget))]
        public string[] IdsApplicationTarget { get; set; }

        [Required]
        public int HoursToReEntryToBarrack { get; set; }

        [Required]
        public int ApplicationDaysInterval { get; set; }

        [Required]
        public int NumberOfSequentialApplication { get; set; }

        [Required]
        public int WettingRecommendedByHectares { get; set; }

        [Required]
        public WaitingHarvestInput[] WaitingToHarvest { get; set; }

        [Required]
        public double DosesQuantityMin { get; set; }

        [Required]
        public double DosesQuantityMax { get; set; }

        public int? WaitingDaysLabel { get; set; }

        [Required]
        public DosesApplicatedTo DosesApplicatedTo { get; set; }

        public bool Active { get; set; }

        public bool Default { get; set; }

    }

    public class WaitingHarvestInput {
        public int WaitingDays { get; set; }
        public string IdCertifiedEntity { get; set; }
        public double Ppm { get; set; }
    }

    public class DosesSwaggerInput {
        public string[] IdVarieties { get; set; }
        public string[] IdSpecies { get; set; }
        public string[] IdsApplicationTarget { get; set; }
        public int HoursToReEntryToBarrack { get; set; }
        public int ApplicationDaysInterval { get; set; }
        public int NumberOfSequentialApplication { get; set; }
        public int WettingRecommendedByHectares { get; set; }
        public WaitingHarvestInput[] WaitingToHarvest { get; set; }
        public double DosesQuantityMin { get; set; }
        public double DosesQuantityMax { get; set; }
        public int? WaitingDaysLabel { get; set; }
        public DosesApplicatedTo DosesApplicatedTo { get; set; }
    }
}