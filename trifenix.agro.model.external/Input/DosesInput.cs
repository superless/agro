using System.Collections.Generic;
using trifenix.agro.db.model.agro.local;

namespace trifenix.agro.model.external.Input
{
    public class DosesInput
    {
        public string[] IdVarieties { get; set; }

        public string[] IdSpecies { get; set; }

        public string[] idsApplicationTarget { get; set; }

        public int DaysToReEntryToBarrack { get; set; }

        
        public int ApplicationDaysInterval { get; set; }


        
        public int NumberOfSecuencialAppication { get; set; }


        
        public int WettingRecommendedByHectares { get; set; }

        private List<WaitingHarvestInput> _waitingToHarvest;

        public List<WaitingHarvestInput> WaitingToHarvest
        {
            get
            {
                _waitingToHarvest = _waitingToHarvest ?? new List<WaitingHarvestInput>();
                return _waitingToHarvest;
            }
            set { _waitingToHarvest = value; }
        }

        public int DosesQuantityMin { get; set; }

        public int DosesQuantityMax { get; set; }

        public int? WaitingDaysLabel { get; set; }

        public DosesApplicatedTo DosesApplicatedTo { get; set; }

    }

    public class WaitingHarvestInput {
        public int WaitingDays { get; set; }
        public string IdCertifiedEntity { get; set; }

    }
}
