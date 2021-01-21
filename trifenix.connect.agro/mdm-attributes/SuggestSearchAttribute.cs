using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.mdm_attributes
{
    /// <summary>
    /// Identifica una propiedad como suggest para incluirla como indice para la busqueda.
    /// </summary>
    public class SuggestSearchAttribute : PropertyIndexAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public SuggestSearchAttribute(StringRelated index) {
            Index = (int)index;
            KindIndex = (int)KindProperty.SUGGESTION;
        }
        
    }


}