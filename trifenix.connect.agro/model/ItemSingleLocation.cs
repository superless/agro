using Microsoft.Azure.Documents.Spatial;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Condicional que determina el metodo de generar
    /// la ubicación del evento
    /// </summary>
    public abstract class ItemSinglesLocation : DocumentLocal
    {

#if CONNECT
        /// <summary>
        /// Ubicación desde donde se generó el evento.
        /// </summary>
        [GeoSearch(GeoRelated.LOCATION_EVENT)]
        public GeoItem Location { get; set; }
#else
        [GeoSearch(GeoRelated.LOCATION_EVENT)]
        public Point Location { get; set; }

#endif


    }

}
