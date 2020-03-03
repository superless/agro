using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.local;

namespace trifenix.agro.model.external.Input
{
    public class BarrackInput : InputBaseName {
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



    public class BarrackSwaggerInput 
    {
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
