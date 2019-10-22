using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "Barrack")]
    public class Barrack : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public string SeasonId { get; set; }

        public string Name { get; set; }

        public PlotLand PlotLand { get; set; }

        public float Hectares { get; set; }

        public int PlantingYear { get; set; }

        public Variety Variety { get; set; }

        public int NumberOfPlants { get; set; }

        private List<GeographicalPoint> _geoPoints;

        public List<GeographicalPoint> GeoPoints
        {
            get {

                _geoPoints = _geoPoints ?? new List<GeographicalPoint>();
                return _geoPoints; }
            set { _geoPoints = value; }
        }

        public Variety Pollinator { get; set; }






    }


}
