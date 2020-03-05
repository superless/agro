using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.db.model.local;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "Barrack")]
    public class Barrack : DocumentBaseName, ISharedCosmosEntity {
        public override string Id { get; set; }

        public override string Name { get; set; }

        public string SeasonId { get; set; }

        public string IdPlotLand { get; set; }

        public double Hectares { get; set; }

        public int PlantingYear { get; set; }

        public string IdVariety { get; set; }

        public int NumberOfPlants { get; set; }

        public GeographicalPoint[] GeographicalPoints { get; set; }


        public string IdPollinator { get; set; }

        public string IdRootstock { get; set; }

    }

}