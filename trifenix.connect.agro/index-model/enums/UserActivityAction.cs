using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace trifenix.connect.agro.index_model.enums
{

    /// <summary>
    /// Actividad que realiza un usuario sobre una entidad
    /// </summary>
    public enum UserActivityAction
    {
        [Description("Creación")]
        CREATE = 0,
        [Description("Modificación")]
        MODIFY = 1,
        [Description("Eliminación")]
        DELETE = 2
    }
}
