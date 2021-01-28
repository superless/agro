using System.ComponentModel;

namespace trifenix.connect.agro.index_model.enums
{
    /// <summary>
    /// Enumeración que determina el estado de un evento fenológico de acuerdo al tiempo próximo en el que este ocurra    
    /// </summary>
    public enum PhenologicalStage
    {
        [Description("grey")]
        Waiting = 0,
        [Description("green")]
        Success = 1,
        [Description("yellow")]
        Warning = 2
    }
}
