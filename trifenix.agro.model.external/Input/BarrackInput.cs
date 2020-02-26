using trifenix.agro.db.model.agro;

namespace trifenix.agro.model.external.Input
{
    public class BarrackInput : InputBaseName {
        public string SeasonId { get; set; }

        public string IdPlotLand { get; set; }

        public float Hectares { get; set; }

        public int PlantingYear { get; set; }

        public string IdVariety { get; set; }

        public int NumberOfPlants { get; set; }

        public GeographicalPoint[] GeographicalPoints { get; set; }

        public string IdPollinator { get; set; }

        public string IdRootstock { get; set; }

    }

    
}
