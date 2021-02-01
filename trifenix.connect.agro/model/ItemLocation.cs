using Microsoft.Azure.Documents.Spatial;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Condicional que determina el metodo de geograficar
    /// del cuartel
    /// </summary>
    public abstract class ItemLocation : DocumentLocal {

#if CONNECT
        /// <summary>
        /// ubicación geográfica del cuartel
        /// </summary>
        [GeoSearch(GeoRelated.LOCATION_BARRACK, Visible = false)]
        public  GeoItem[] GeographicalPoints { get; set; }
#else
        [GeoSearch(GeoRelated.LOCATION_BARRACK, Visible = false)]
        public Point[] GeographicalPoints { get; set; } 
#endif



    }
}