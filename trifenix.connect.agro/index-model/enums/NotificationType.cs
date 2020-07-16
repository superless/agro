using System.ComponentModel;

namespace trifenix.connect.agro.index_model.enums
{
    /// <summary>
    /// Tipos de notificaciones
    /// </summary>
    public enum NotificationType
    {
        [Description("otro")]
        Default = 1,

        [Description("Evento fenológico")]
        Phenological = 0,
    }
}
