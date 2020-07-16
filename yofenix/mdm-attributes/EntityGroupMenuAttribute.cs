using System;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm.ts_model;

namespace trifenix.connect.mdm_attributes
{

    /// <summary>
    /// indica si una clase es agrupada para construir un menú.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class , AllowMultiple = true)]
    public class EntityGroupMenuAttribute : Attribute {
        public EntityGroupMenuAttribute(int menuId, PhisicalDevice device, int submenu )
        {
            Grupo = new GroupMenu
            {
                Device = device,
                MenuIndex = menuId,
                SubMenuIndex  = submenu
            };
        }


        public GroupMenu Grupo { get; set; }
    }




}
