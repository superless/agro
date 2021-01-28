using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.mdm_attributes
{
    /// <summary>
    /// Indica si la propiedad de una clase es numérica.
    /// </summary>
    public class Num32SearchAttribute : PropertyIndexAttribute
    {
        private readonly NumRelated _index;
        public Num32SearchAttribute(NumRelated index) {
            Index = (int)index;
            KindIndex = (int)KindProperty.NUM32;
        }
        
    }


}