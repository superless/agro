﻿using System.Collections.Generic;

namespace trifenix.agro.db.model.local {
    public class Doses {
        
        private List<Variety> _varieties;

        /// <summary>
        /// variedades que aplica la dosis (opcional)
        /// </summary>
        public List<Variety> Varieties
        {
            get {
                _varieties = _varieties ?? new List<Variety>();
                return _varieties; }
            set { _varieties = value; }
        }


        /// <summary>
        /// Especie que aplica 
        /// </summary>
        

        private List<ApplicationTarget> _target;

        public List<ApplicationTarget> Targets
        {
            get {
                _target = _target ?? new List<ApplicationTarget>();
                return _target; }
            set { _target = value; }
        }


        /// <summary>
        /// Dias necesario para reingresar al cuartel.
        /// </summary>
        public int DaysToReEntryToBarrack { get; set; }


        private List<Specie> _species;

        /// <summary>
        /// Dias para cosechar por entidad certificadora
        /// </summary>
        public List<Specie> Species
        {
            get
            {
                _species = _species ?? new List<Specie>();
                return _species;
            }
            set { _species = value; }
        }

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

        /// <summary>
        /// intervalo de aplicación, días entre aplicación, 0 no aplica.
        /// </summary>
        public int ApplicationDaysInterval { get; set; }


        /// <summary>
        /// número de aplicaciones secuenciales (1 por defecto).
        /// </summary>
        public int NumberOfSequentialApplication { get; set; }


        /// <summary>
        /// Mojamiento recomendado por hectarea
        /// </summary>
        public int WettingRecommendedByHectares { get; set; }


        public int DosesQuantityMin { get; set; }

        public int DosesQuantityMax { get; set; }

        public int? WaitingDaysLabel { get; set; }

        public DosesApplicatedTo DosesApplicatedTo { get; set; }

        public string IdSeason { get; set; }
    }

    public enum DosesApplicatedTo {
        L100 = 0,
        L1000 = 1

    }


}