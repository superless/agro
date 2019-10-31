using System.Collections.Generic;

namespace trifenix.agro.model.external.Input
{
    public class DosesInput
    {
        public string[] IdVarieties { get; set; }

        public string IdSpecie { get; set; }

        public string[] IdsSickness { get; set; }

        public int DaysToReEntryToBarrack { get; set; }

        
        public int ApplicationDaysInterval { get; set; }


        
        public int NumberOfSecuencialAppication { get; set; }


        
        public int WettingRecommended { get; set; }

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

    }

    public class WaitingHarvestInput {
        public int WaitingDays { get; set; }

        public bool IsLabel { get; set; }

        public string IdCertifiedEntity { get; set; }

    }
}
