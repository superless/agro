using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.local;

namespace trifenix.agro.model.external.Input
{
    public class BarrackInput : InputBaseName {
        [Required,Reference(typeof(Season))]
        public string SeasonId { get; set; }

        [Required, Reference(typeof(PlotLand))]
        public string IdPlotLand { get; set; }

        [Required]
        public double Hectares { get; set; }

        [Required]
        public int PlantingYear { get; set; }

        [Required,Reference(typeof(Variety))]
        public string IdVariety { get; set; }

        [Required]
        public int NumberOfPlants { get; set; }

        public GeographicalPointInput[] GeographicalPoints { get; set; }

        [Reference(typeof(Variety))]
        public string IdPollinator { get; set; }

        [Reference(typeof(Rootstock))]
        public string IdRootstock { get; set; }

    }

    public class BarrackSwaggerInput {
        [Required]
        public string SeasonId { get; set; }

        [Required]
        public string IdPlotLand { get; set; }

        [Required]
        public double Hectares { get; set; }

        [Required]
        public int PlantingYear { get; set; }

        [Required]
        public string IdVariety { get; set; }

        [Required]
        public string Name { get; set; }

        public int NumberOfPlants { get; set; }

        public GeographicalPoint[] GeographicalPoints { get; set; }

        public string IdPollinator { get; set; }

        public string IdRootstock { get; set; }

    }

}