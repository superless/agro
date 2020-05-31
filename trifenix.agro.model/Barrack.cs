using Cosmonaut;
using Cosmonaut.Attributes;
using Microsoft.Azure.Documents.Spatial;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "Barrack")]
    [ReferenceSearchHeader(EntityRelated.BARRACK, Kind =EntityKind.ENTITY, PathName ="barracks")]
    public class Barrack : DocumentBaseName<long>, ISharedCosmosEntity {
        public override string Id { get; set; }

        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }



        [Group(0, Device.WEB, 6)]
        [StringSearch(StringRelated.GENERIC_NAME)]
        [Unique]
        [Required]
        public override string Name { get; set; }


        [Group(0, Device.WEB, 6)]
        [ReferenceSearch(EntityRelated.PLOTLAND)]        
        public string IdPlotLand { get; set; }


        [Group(1, Device.WEB, 3)]
        [DoubleSearch(DoubleRelated.HECTARES)]
        public double Hectares { get; set; }

        [Group(1, Device.WEB, 3)]
        [Num32Search(NumRelated.PLANTING_YEAR)]
        public int PlantingYear { get; set; }

        [Group(1, Device.WEB, 3)]
        [Num32Search(NumRelated.NUMBER_OF_PLANTS)]
        public int NumberOfPlants { get; set; }


        [Group(2, Device.WEB, 3)]
        [ReferenceSearch(EntityRelated.VARIETY)]
        public string IdVariety { get; set; }

        [Group(2, Device.WEB, 3)]
        [ReferenceSearch(EntityRelated.POLLINATOR)]
        public string IdPollinator { get; set; }

        [Group(2, Device.WEB, 6)]
        [ReferenceSearch(EntityRelated.ROOTSTOCK)]
        public string IdRootstock { get; set; }


        [Group(3, Device.WEB, 6)]
        [ReferenceSearch(EntityRelated.SEASON)]
        public string SeasonId { get; set; }

        [GeoSearch(GeoRelated.LOCATION_BARRACK, Visible = false)]
        public Point[] GeographicalPoints { get; set; }

    }

}