using System;

namespace trifenix.agro.model.external.Input
{
    public class SeasonInput : InputBase {

        public DateTime  StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool? Current { get; set; }

        public string IdCostCenter { get; set; }


    }
}
