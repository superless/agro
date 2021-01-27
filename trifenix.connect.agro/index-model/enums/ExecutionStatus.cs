using System.ComponentModel;

namespace trifenix.connect.agro.index_model.enums
{
    /// <summary>
    /// Determina las distintas etapas de la ejecución de una orden.
    /// </summary>
    public enum ExecutionStatus
    {
        [Description("En planificación")]
        Planification = 0,
        [Description("En progreso")]
        InProcess = 1,

        [Description("Terminado")]
        EndProcess = 2,

        [Description("Cerrado")]
        Closed = 3

    }
}
