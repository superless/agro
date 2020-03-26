using Cosmonaut;
using Cosmonaut.Attributes;
using Microsoft.Azure.Documents.Spatial;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.agro {

    [SharedCosmosCollection("agro", "Barrack")]
    [ReferenceSearch(EntityRelated.BARRACK)]
    public class Barrack : DocumentBaseName, ISharedCosmosEntity {
        public override string Id { get; set; }

        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [ReferenceSearch(EntityRelated.SEASON)]
        public string SeasonId { get; set; }

        [ReferenceSearch(EntityRelated.PLOTLAND)]
        public string IdPlotLand { get; set; }

        
        public double Hectares { get; set; }

        public int PlantingYear { get; set; }

        public string IdVariety { get; set; }

        public int NumberOfPlants { get; set; }

        public Point[] GeographicalPoints { get; set; }


        public string IdPollinator { get; set; }

        public string IdRootstock { get; set; }

    }

}