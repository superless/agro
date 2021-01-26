using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.mdm_attributes
{
    /// <summary>
    /// Atributo que identifica una propiedad como string.
    /// </summary>
    public class StringSearchAttribute : PropertyIndexAttribute
    {
        
        public StringSearchAttribute(StringRelated index) {
            Index = (int)index;
            KindIndex = (int)KindProperty.STR;
        }
        
    }


}