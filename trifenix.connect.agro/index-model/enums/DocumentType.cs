using System.ComponentModel;

namespace trifenix.connect.agro.index_model.enums
{
    /// <summary>
    /// Determina el tipo de documento 
    /// </summary>

    public enum DocumentType
    {
        [Description("Factura")]
        FACTURA = 0,

        [Description("Compra alternativa")]
        ALTERNATIVA = 1,

        [Description("Productos que pasan al stock de la siguiente temporada")]
        NUEVATEMPORADA = 2
    }
}
