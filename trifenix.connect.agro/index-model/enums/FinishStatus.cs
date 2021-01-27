using System.ComponentModel;

namespace trifenix.connect.agro.index_model.enums
{
    /// <summary>
    /// Determina el cierre de una ejecución de una orden por parte del administrador.
    /// </summary>
    public enum FinishStatus
    {
        [Description("aún no termina")]
        NotFinish = 0,

        [Description("completado ok")]
        Successful = 1,

        [Description("incompleto")]
        InComplete = 2,

        [Description("cancelado")]
        Cancelled = 3
    }
}
