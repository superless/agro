using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.mdm_attributes
{
    /// <summary>
    /// Determina si una propiedad es de tipo enum
    /// </summary>
    public class EnumSearchAttribute : PropertyIndexAttribute
    {
        
        public EnumSearchAttribute(EnumRelated index) {
            Index = (int)index;
            KindIndex = (int)KindProperty.ENUM;
        }
        
    }


}