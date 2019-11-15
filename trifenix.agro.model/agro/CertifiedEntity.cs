using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model.agro
{

    /// <summary>
    /// Entidad certificadora, encargada de validar el proceso de exportación
    /// </summary>
    [SharedCosmosCollection("agro", "CertifiedEntity")]
    public class CertifiedEntity : DocumentBase, ISharedCosmosEntity
    {

        /// <summary>
        /// Identificador de la entidad
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Nombre de la entidad certificadora.
        /// </summary>
        public string Name { get; set; }

        public string Abbreviation { get; set; }
    }
}
