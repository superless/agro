using System.ComponentModel;

namespace trifenix.connect.agro.index_model.enums
{
    /// <summary>
    /// Determina el tipo de pre orden
    /// </summary>
    public enum PreOrderType
    {
        [Description("Pre-Orden por Defecto")]
        DEFAULT = 0,
        [Description("Pre-Orden Fenológica")]
        PHENOLOGICAL = 1
    }
}
