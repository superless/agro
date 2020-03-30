using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.db.model;
using trifenix.agro.enums;
using trifenix.agro.enums.model;
using trifenix.agro.enums.searchModel;

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