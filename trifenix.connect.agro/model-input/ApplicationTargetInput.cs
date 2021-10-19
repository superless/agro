using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Objetivo de una aplicación
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.TARGET)]
    public class ApplicationTargetInput : LocalBase
    {

        /// <summary>
        /// Abreviación 
        /// </summary>
        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        [Required, Unique]
        public string Abbreviation { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [StringSearch(StringRelated.GENERIC_NAME)]
        [Required, Unique]
        public string Name { get; set; }

    }

   

}