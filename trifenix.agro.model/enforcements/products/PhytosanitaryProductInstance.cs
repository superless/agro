using System;

namespace trifenix.agro.db.model.enforcements.products
{
    /// <summary>
    /// Instance of Phytosanitary product applied to an ApplicationOrder    
    /// </summary>
    public class PhytosanitaryProductInstance {
        public PhytosanitaryProduct PhytosanitaryProduct { get; set; }

        public DoseTemplate DosesTemplate { get; set; }


    }


}
