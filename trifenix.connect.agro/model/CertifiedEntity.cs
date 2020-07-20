using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.db;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro_model
{

    /// <summary>
    /// Entidad certificadora, encargada de validar el proceso de exportación
    /// </summary>
    [SharedCosmosCollection("agro", "CertifiedEntity")]
    [ReferenceSearchHeader(EntityRelated.CERTIFIED_ENTITY, PathName ="certified_entities", Kind = EntityKind.ENTITY)]    
    public class CertifiedEntity : DocumentBaseName<long>, ISharedCosmosEntity {

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


        /// <summary>
        /// Identificador visual de la entidad certificadora
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }



        /// <summary>
        /// Abreviación de la
        /// </summary>
        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }

    }

}