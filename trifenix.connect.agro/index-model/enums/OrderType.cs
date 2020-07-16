using System.ComponentModel;

namespace trifenix.connect.agro.index_model.enums
{
    /// <summary>
    /// determina el tipo de orden.
    /// </summary>
    public enum OrderType
    {
        [Description("Orden por Defecto")]
        DEFAULT = 0,
        [Description("Orden Fenológica")]
        PHENOLOGICAL = 1
    }
}
