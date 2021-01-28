using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.input;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de entidad certificadora
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.CERTIFIED_ENTITY)]
    public class CertifiedEntityInput : InputBase {

        /// <summary>
        /// Abreviación
        /// </summary>
        [Required,Unique]
        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }
    }

    

}