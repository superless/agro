using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.model;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Condicional que determina el metodo de generar
    /// la ubicación del evento
    /// </summary>
    public abstract class ItemSinglesLocation : DocumentDb
    {

        [GeoSearch(GeoRelated.LOCATION_EVENT)]
        public GeoItem Location { get; set; }


    }

}
