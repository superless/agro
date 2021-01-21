using System.ComponentModel;

namespace trifenix.connect.agro.index_model.enums
{
    /// <summary>
    /// Tipo de contenedor del producto
    /// </summary>
    public enum KindOfProductContainer
    {
        [Description("Botella")]
        Bottle = 0,
        [Description("Caja")]
        Box = 1,
        [Description("Lata")]
        Can = 2
    }
}
