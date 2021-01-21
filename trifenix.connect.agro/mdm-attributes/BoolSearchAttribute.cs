using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.mdm_attributes
{
    /// <summary>
    /// Determina si la propiedad de una clase es booleana.
    /// </summary>
    public class BoolSearchAttribute : PropertyIndexAttribute
    {
        private readonly BoolRelated _index;
        public BoolSearchAttribute(BoolRelated index) {
            Index = (int)index;
            KindIndex = (int)KindProperty.BOOL;
        }
       
    }


}