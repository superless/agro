using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.mdm_attributes
{
    /// <summary>
    /// Permite agrupar entidades para construir menús en el cliente.
    /// </summary>
    public class GroupMenuAttribute : EntityGroupMenuAttribute {

        public GroupMenuAttribute(MenuEntityRelated menu, PhisicalDevice device, SubMenuEntityRelated submenu) : base((int)menu, device, (int)submenu)
        {

        }
    }


}