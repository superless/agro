using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de porta injertos
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.ROOTSTOCK)]
    public class RootstockInput : LocalBase
    {

        /// <summary>
        /// Nombre
        /// </summary>
        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        /// <summary>
        /// Abreviación
        /// </summary>
        [Required, Unique]
        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }

    }

    

}