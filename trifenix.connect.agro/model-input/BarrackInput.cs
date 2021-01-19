using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.input;
using trifenix.connect.mdm.validation_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{
    [ReferenceSearchHeader(EntityRelated.BARRACK)]
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
        public GeoItem[] GeographicalPoints { get; set; }

        [ReferenceSearch(EntityRelated.POLLINATOR,EntityRelated.POLLINATOR)]
        [Reference(typeof(Variety))]
        public string IdPollinator { get; set; }


        [ReferenceSearch(EntityRelated.ROOTSTOCK)]
        [Reference(typeof(Rootstock))]
        public string IdRootstock { get; set; }

    }

   

}