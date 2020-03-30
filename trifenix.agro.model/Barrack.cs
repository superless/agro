using Cosmonaut;
using Cosmonaut.Attributes;
using Microsoft.Azure.Documents.Spatial;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "Barrack")]
    [ReferenceSearch(EntityRelated.BARRACK)]
    public class Barrack : DocumentBaseName, ISharedCosmosEntity {
        public override string Id { get; set; }

        [SuggestSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [ReferenceSearch(EntityRelated.SEASON)]
        public string SeasonId { get; set; }

        [ReferenceSearch(EntityRelated.PLOTLAND)]
        public string IdPlotLand { get; set; }

        [DoubleSearch(DoubleRelated.HECTARES)]
        public double Hectares { get; set; }

        [Num32Search(NumRelated.PLANTING_YEAR)]
        public int PlantingYear { get; set; }

        [ReferenceSearch(EntityRelated.VARIETY)]
        public string IdVariety { get; set; }

        [Num32Search(NumRelated.NUMBER_OF_PLANTS)]
        public int NumberOfPlants { get; set; }

        [GeoSearch(GeoRelated.GENERIC_LOCATION)]
        public Point[] GeographicalPoints { get; set; }

        [ReferenceSearch(EntityRelated.POLLINATOR)]
        public string IdPollinator { get; set; }

        [ReferenceSearch(EntityRelated.ROOTSTOCK)]
        public string IdRootstock { get; set; }

    }

}