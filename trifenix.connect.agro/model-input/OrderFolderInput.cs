using System.ComponentModel.DataAnnotations;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.input;
using trifenix.connect.mdm.validation_attributes;

namespace trifenix.connect.agro_model_input
{


    [ReferenceSearchHeader(EntityRelated.ORDER_FOLDER)]
    public class OrderFolderInput : InputBase {

        [ReferenceSearch(EntityRelated.PHENOLOGICAL_EVENT)]
        [Required, Reference(typeof(PhenologicalEvent))]
        public string IdPhenologicalEvent { get; set; }



        [ReferenceSearch(EntityRelated.TARGET)]
        [Required, Reference(typeof(ApplicationTarget))]
        public string IdApplicationTarget { get; set; }


        [ReferenceSearch(EntityRelated.SPECIE)]
        [Required, Reference(typeof(Specie))]
        public string IdSpecie { get; set; }

        [ReferenceSearch(EntityRelated.INGREDIENT)]
        [Reference(typeof(Ingredient))]
        public string IdIngredient { get; set; }


        [ReferenceSearch(EntityRelated.CATEGORY_INGREDIENT)]
        [Required, Reference(typeof(IngredientCategory))]
        public string IdIngredientCategory { get; set; }

    }

   
}