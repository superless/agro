using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.mdm_attributes
{
    /// <summary>
    /// Determina si la propiedad es de tipo double.
    /// </summary>
    public class DoubleSearchAttribute : PropertyIndexAttribute
    {
        
        public DoubleSearchAttribute(DoubleRelated index) {
            Index = (int)index;
            KindIndex = (int)KindProperty.DBL;
        }
       
    }


}