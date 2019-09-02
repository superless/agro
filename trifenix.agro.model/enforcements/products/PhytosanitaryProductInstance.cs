using System;

namespace trifenix.agro.model.enforcements.products
{
    /// <summary>
    /// Instance of Phytosanitary product applied to an ApplicationOrder    
    /// </summary>
    public class PhytosanitaryProductInstance {
        public PhytosanitaryProduct PhytosanitaryProduct { get; set; }

        /// <summary>
        /// Dose in cc or kg by 100Lts of water
        /// </summary>
        public int DoseBy100 { get; set; }

        /// <summary>
        /// Date Creation of the template
        /// </summary>
        public DateTime DateCreation { get; set; }

        public MeausereType MeausereType { get; set; }


        public Cadence Cadence { get; set; }
    }


}
