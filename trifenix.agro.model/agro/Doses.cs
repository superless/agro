using Cosmonaut;
using Cosmonaut.Attributes;
using System.Collections.Generic;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.enums;

namespace trifenix.agro.db.model.agro
{



    [SharedCosmosCollection("agro", "Doses")]
    public class Doses : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }


        public string[] IdVarieties { get; set; }

        public string[] IdSpecies { get; set; }

        public string[] idsApplicationTarget { get; set; }

        public int DaysToReEntryToBarrack { get; set; }


        public int ApplicationDaysInterval { get; set; }



        public int NumberOfSequentialApplication { get; set; }



        public int WettingRecommendedByHectares { get; set; }

        public double DosesQuantityMin { get; set; }

        public double DosesQuantityMax { get; set; }

        public int? WaitingDaysLabel { get; set; }

        public DosesApplicatedTo DosesApplicatedTo { get; set; }




        private List<WaitingHarvest> _waitingToHarvest;

        /// <summary>
        /// Dias para cosechar por entidad certificadora
        /// </summary>
        public List<WaitingHarvest> WaitingToHarvest
        {
            get
            {
                _waitingToHarvest = _waitingToHarvest ?? new List<WaitingHarvest>();
                return _waitingToHarvest;
            }
            set { _waitingToHarvest = value; }
        }



        

    }

    


}