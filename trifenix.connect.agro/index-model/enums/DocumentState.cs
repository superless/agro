using System.ComponentModel;

namespace trifenix.connect.agro.index_model.enums
{
    /// <summary>
    /// Determina el estado de documento 
    /// </summary>

    public enum DocumentState
    {
        [Description("En espera")]
        ENESPERA = 0,

        [Description("En proceso")]
        ENPROCESO = 1,

        [Description("Aceptado")]
        ACEPTADO = 2,

        [Description("Cancelado")]
        CANCELADO = 3,
    }
}