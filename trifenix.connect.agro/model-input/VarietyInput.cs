using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.input;
using trifenix.connect.mdm.validation_attributes;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de variedad de una especie
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.VARIETY)]
    public class VarietyInput : InputBase {

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
        [ReferenceSearch(EntityRelated.SPECIE)]
        [Required, Reference(typeof(Specie))]
        public string IdSpecie { get; set; }

    }


}