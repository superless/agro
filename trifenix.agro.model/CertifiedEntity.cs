using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.agro {

    /// <summary>
    /// Entidad certificadora, encargada de validar el proceso de exportación
    /// </summary>
    [SharedCosmosCollection("agro", "CertifiedEntity")]
    [ReferenceSearch(EntityRelated.CERTIFIED_ENTITY)]
    public class CertifiedEntity : DocumentBaseName, ISharedCosmosEntity {

        /// <summary>
        /// Identificador de la entidad
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Nombre de la entidad certificadora.
        /// </summary>
        /// 
        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }

    }

}