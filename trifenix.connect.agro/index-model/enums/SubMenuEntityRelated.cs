using System.ComponentModel;

namespace trifenix.connect.agro.index_model.enums
{
    /// <summary>
    /// Determina el menú para ser utilizado como metadata
    /// </summary>
    public enum SubMenuEntityRelated
    {
        [Description("Gestión")]
        GESTION = 0,
        [Description("Terreno")]
        TERRENO = 1,
        [Description("Orden de aplicación")]
        ORDEN_APLICACION = 2,
        [Description("Usuarios")]
        USUARIOS = 3,
        [Description("Producto")]
        PRODUCTO = 4,
        [Description("Maquinaria")]
        MAQUINARIA = 5,
        [Description("Especies")]
        ESPECIES = 6,
    }
}
