using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.validation_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de cuarteles
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.BARRACK)]
    public class BarrackInput : LocalBase
    {

        /// <summary>
        /// Nombre del cuartel
        /// </summary>
        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Busqueda por referencia de una temporada
        /// </summary>
        [ReferenceSearch(EntityRelated.SEASON)]
        [Required, Reference(typeof(Season))]
        public string IdSeason { get; set; }

        /// <summary>
        /// Busqueda por referencia de una parcela
        /// </summary>
        [ReferenceSearch(EntityRelated.PLOTLAND)]
        [Required, Reference(typeof(PlotLand))]
        public string IdPlotLand { get; set; }

        /// <summary>
        /// Hectareas del cuartel
        /// </summary>
        [Required]
        [DoubleSearch(DoubleRelated.HECTARES)]
        public double Hectares { get; set; }

        /// <summary>
        /// Año de plantación
        /// </summary>
        [Required]
        [Num32Search(NumRelated.PLANTING_YEAR)]
        public int PlantingYear { get; set; }

        /// <summary>
        /// Búsqueda por referencia de una variedad
        /// </summary>
        [Required, Reference(typeof(Variety))]
        [ReferenceSearch(EntityRelated.VARIETY)]
        public string IdVariety { get; set; }

        /// <summary>
        /// Número de plantas del cuartel
        /// </summary>
        [Required]
        [Num32Search(NumRelated.NUMBER_OF_PLANTS)]
        public int NumberOfPlants { get; set; }

        /// <summary>
        /// Geolocalización del cuartel
        /// </summary>
        [GeoSearch(GeoRelated.LOCATION_BARRACK)]
        public GeoItem[] GeographicalPoints { get; set; }

        /// <summary>
        /// Búsqueda por referencia de un polinizador
        /// </summary>
        [ReferenceSearch(EntityRelated.POLLINATOR, EntityRelated.POLLINATOR)]
        [Reference(typeof(Variety))]
        public string IdPollinator { get; set; }

        /// <summary>
        /// Búsqueda por referencia de un porta injerto
        /// </summary>
        [ReferenceSearch(EntityRelated.ROOTSTOCK)]
        [Reference(typeof(Rootstock))]
        public string IdRootstock { get; set; }

    }



}
