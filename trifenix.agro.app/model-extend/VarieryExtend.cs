using trifenix.connect.agro_model;

namespace trifenix.agro.app.model_extend
{
    public class VarieryExtend : Variety {

        public string FullName { get; set; }
    }

    public class WaitingHarvestExtend : WaitingHarvest {
        public string FullName { get; set; }

    }
}
