using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.mdm_attributes
{
    /// <summary>
    /// Indica si un campo es de tipo geo.
    /// </summary>
    public class GeoSearchAttribute : PropertyIndexAttribute
    {
        
        public GeoSearchAttribute(GeoRelated index) {
            Index = (int)index;
            KindIndex = (int)KindProperty.GEO;
        }
       
    }


}