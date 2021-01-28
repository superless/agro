using System.ComponentModel;

namespace trifenix.connect.agro.index_model.enums
{
    /// <summary>
    /// Determina el tipo de cierre de una ejecución de una orden.
    /// </summary>
    public enum ClosedStatus
    {
        [Description("Sin cerrar aún")]
        NotClosed = 0,

        [Description("Cierre Exitoso")]
        Successful = 1,

        [Description("Cierre Fallido")]
        Failed = 2,

        [Description("Recalendarizado")]
        Rescheduled = 3
    }
}
