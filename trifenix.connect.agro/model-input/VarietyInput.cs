using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.validation_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de variedad de una especie
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.VARIETY)]
    public class VarietyInput : LocalBase
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

        /// <summary>
        /// Búsqueda por referencia de la especie asociada
        /// </summary>
        [Required, Reference(typeof(Specie))]
        [ReferenceSearch(EntityRelated.SPECIE)] 
        public string IdSpecie { get; set; }

    }


}