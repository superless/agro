using Microsoft.Spatial;
using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.mdm.search.model
{
    /// <summary>
    /// Propiedad de tipo Coordenada,
    /// esto permite realizar busqueda de mapas en azure search.
    /// </summary>
    public class GeoProperty : BaseProperty<GeographyPoint>, IProperty<GeographyPoint> { }
}
