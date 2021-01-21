using System.ComponentModel;

namespace trifenix.connect.agro.index_model.enums
{
    /// <summary>
    /// Determina si la aplicación se debe a hacer en base a que cantidades.
    /// </summary>
    public enum DosesApplicatedTo
    {
        [Description("100 litros")]
        L100 = 0,

        [Description("1000 litros")]
        L1000 = 1

    }
}
