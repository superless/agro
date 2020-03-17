using trifenix.agro.enums;

namespace trifenix.agro.model.external.Input {
    public class DosesInput : InputBase {

        public string IdProduct { get; set; }

        public string[] IdVarieties { get; set; }

        public string[] IdSpecies { get; set; }

        public string[] idsApplicationTarget { get; set; }

        public int DaysToReEntryToBarrack { get; set; }

        public int ApplicationDaysInterval { get; set; }
        
        public int NumberOfSequentialApplication { get; set; }
        
        public int WettingRecommendedByHectares { get; set; }

        public WaitingHarvestInput[] WaitingToHarvest;

        public double DosesQuantityMin { get; set; }

        public double DosesQuantityMax { get; set; }

        public int? WaitingDaysLabel { get; set; }

        public DosesApplicatedTo DosesApplicatedTo { get; set; }

    }

    public class WaitingHarvestInput {
        public int WaitingDays { get; set; }
        public string IdCertifiedEntity { get; set; }

    }
}
