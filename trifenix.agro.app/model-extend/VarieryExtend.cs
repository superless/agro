using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;

namespace trifenix.agro.app.model_extend
{
    public class VarieryExtend : Variety {

        public string FullName { get; set; }
    }

    public class WaitingHarvestExtend : WaitingHarvest {
        public string FullName { get; set; }

    }

    public class DoseExtend : DosesInput {
        public string SpeciesList { get; set; }

        public string VarietyList { get; set; }

        public string TargetList { get; set; }



    }
}
