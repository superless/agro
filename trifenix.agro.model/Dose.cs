using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using trifenix.agro.attr;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.enums;
using trifenix.agro.enums.model;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.agro {

    [SharedCosmosCollection("agro", "Dose")]
    [ReferenceSearch(EntityRelated.DOSES)]
    public class Dose : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }

        [Num32Search(NumRelated.GENERIC_CORRELATIVE)]
        public long Correlative { get; set; }

        [DateSearch(DateRelated.LAST_MODIFIED)]
        public DateTime LastModified { get; set; }

        [ReferenceSearch(EntityRelated.PRODUCT)]
        public string IdProduct { get; set; }

        [ReferenceSearch(EntityRelated.VARIETY)]
        public string[] IdVarieties { get; set; }

        [ReferenceSearch(EntityRelated.SPECIE)]
        public string[] IdSpecies { get; set; }

        [ReferenceSearch(EntityRelated.TARGET)]
        public string[] IdsApplicationTarget { get; set; }

        [Num32Search(NumRelated.HOURS_TO_ENTRY)]
        public int HoursToReEntryToBarrack { get; set; }

        [Num32Search(NumRelated.DAYS_INTERVAL)]
        public int ApplicationDaysInterval { get; set; }

        [Num32Search(NumRelated.NUMBER_OF_SECQUENTIAL_APPLICATION)]
        public int NumberOfSequentialApplication { get; set; }

        [Num32Search(NumRelated.WETTING_RECOMMENDED)]
        public int WettingRecommendedByHectares { get; set; }

        [DoubleSearch(DoubleRelated.QUANTITY_MIN)]
        public double DosesQuantityMin { get; set; }

        [DoubleSearch(DoubleRelated.QUANTITY_MAX)]
        public double DosesQuantityMax { get; set; }

        [Num32Search(NumRelated.WAITING_DAYS)]
        public int? WaitingDaysLabel { get; set; }

        [EnumSearch(EnumRelated.DOSES_APPLICATED_TO)]
        public DosesApplicatedTo DosesApplicatedTo { get; set; }


        private List<WaitingHarvest> _waitingToHarvest;

        /// <summary>
        /// Dias para cosechar por entidad certificadora
        /// </summary>
        [ReferenceSearch(EntityRelated.WAITINGHARVEST, true)]
        public List<WaitingHarvest> WaitingToHarvest {
            get {
                _waitingToHarvest = _waitingToHarvest ?? new List<WaitingHarvest>();
                return _waitingToHarvest;
            }
            set { _waitingToHarvest = value; }
        }

        [BoolSearch(BoolRelated.GENERIC_DEFAULT)]
        public bool Default { get; set; }

        [BoolSearch(BoolRelated.GENERIC_ACTIVE)]
        public bool Active { get; set; }

    }

}