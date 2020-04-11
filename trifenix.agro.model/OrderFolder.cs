using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model
{

    [SharedCosmosCollection("agro", "OrderFolder")]
    [ReferenceSearch(EntityRelated.ORDER_FOLDER)]
    public class OrderFolder : DocumentBase, ISharedCosmosEntity
    {


        public override string Id { get; set; }


        [ReferenceSearch(EntityRelated.PHENOLOGICAL_EVENT)]
        public string IdPhenologicalEvent { get; set; }


        [ReferenceSearch(EntityRelated.TARGET)]
        public string IdApplicationTarget { get; set; }


        [ReferenceSearch(EntityRelated.SPECIE)]
        public string IdSpecie { get; set; }


        [ReferenceSearch(EntityRelated.INGREDIENT)]
        public string  IdIngredient { get; set; }


        [ReferenceSearch(EntityRelated.CATEGORY_INGREDIENT)]
        public string IdIngredientCategory { get; set; }
    }
}