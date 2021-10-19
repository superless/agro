using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{
    /// <summary>
    /// Ingreso de carpeta de ordenes
    /// </summary>
    [ReferenceSearchHeader(EntityRelated.ORDER_FOLDER)]
    public class OrderFolderInput : LocalBase
    {

        /// <summary>
        /// Búsqueda por referencia del evento fenológico asociado
        /// </summary>
        [ReferenceSearch(EntityRelated.PHENOLOGICAL_EVENT)]
        [Required, Reference(typeof(PhenologicalEvent))]
        public string IdPhenologicalEvent { get; set; }

        /// <summary>
        /// Búsqueda por referencia del objetivo de aplicación asociado
        /// </summary>
        [ReferenceSearch(EntityRelated.TARGET)]
        [Required, Reference(typeof(ApplicationTarget))]
        public string IdApplicationTarget { get; set; }

        /// <summary>
        /// Búsqueda por referencia de la especie asociada
        /// </summary>
        [ReferenceSearch(EntityRelated.SPECIE)]
        [Required, Reference(typeof(Specie))]
        public string IdSpecie { get; set; }

        /// <summary>
        /// Búsqueda por referencia del ingrediente asociado
        /// </summary>
        [ReferenceSearch(EntityRelated.INGREDIENT)]
        [Reference(typeof(Ingredient))]
        public string IdIngredient { get; set; }

    }

   
}