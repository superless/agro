using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.mdm_attributes
{
    /// <summary>
    /// Determina si la propiedad es una fecha.
    /// </summary>
    public class DateSearchAttribute : PropertyIndexAttribute
    {
       
        public DateSearchAttribute(DateRelated index) {
            Index = (int)index;
            KindIndex = (int)KindProperty.DATE;
        }
       
    }


}