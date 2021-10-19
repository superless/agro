using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.model;

namespace trifenix.connect.agro_model
{
    /// <summary>
    /// Condicional que determina el metodo de geograficar
    /// del cuartel
    /// </summary>
    public abstract class ItemLocation : DocumentDb {


    [GeoSearch(GeoRelated.LOCATION_BARRACK, Visible = false)]
    public GeoItem[] GeographicalPoints { get; set; }




    }
}