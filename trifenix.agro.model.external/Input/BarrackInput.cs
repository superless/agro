using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.db.model;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input
{
    [ReferenceSearch(EntityRelated.BARRACK)]
    public class BarrackInput : InputBase {

        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }


        [ReferenceSearch(EntityRelated.SEASON)]
        [Required,Reference(typeof(Season))]
        public string SeasonId { get; set; }


        [ReferenceSearch(EntityRelated.PLOTLAND)]
        [Required, Reference(typeof(PlotLand))]
        public string IdPlotLand { get; set; }

        [Required]
        [DoubleSearch(DoubleRelated.HECTARES)]
        public double Hectares { get; set; }

        [Required]
        [Num32Search(NumRelated.PLANTING_YEAR)]
        public int PlantingYear { get; set; }

        [Required,Reference(typeof(Variety))]
        [ReferenceSearch(EntityRelated.VARIETY)]
        public string IdVariety { get; set; }

        [Required]
        [Num32Search(NumRelated.NUMBER_OF_PLANTS)]
        public int NumberOfPlants { get; set; }

        [GeoSearch(GeoRelated.LOCATION_BARRACK)]
        public GeographicalPointInput[] GeographicalPoints { get; set; }

        [ReferenceSearch(EntityRelated.POLLINATOR)]
        [Reference(typeof(Variety))]
        public string IdPollinator { get; set; }


        [ReferenceSearch(EntityRelated.ROOTSTOCK)]
        [Reference(typeof(Rootstock))]
        public string IdRootstock { get; set; }

    }

   

}