using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.mdm_attributes
{
    /// <summary>
    /// Determina si una propiedad es de tipo long.
    /// </summary>
    public class Num64SearchAttribute : PropertyIndexAttribute
    {
        
        public Num64SearchAttribute(NumRelated index) {
            Index = (int)index;
            KindIndex = (int)KindProperty.NUM64;
        }
    }


}