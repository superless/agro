using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "OrderFolder")]
    public class OrderFolder : DocumentBase, ISharedCosmosEntity
    {


        public override string Id { get; set; }


        [ReferenceSearch(EntityRelated.PHENOLOGICAL_EVENT)]
        public string IdPhenologicalEvent { get; set; }


        [ReferenceSearch(EntityRelated.TARGET)]
        public string IdApplicationTarget { get; set; }


        [ReferenceSearch(EntityRelated.TARGET)]
        public string IdSpecie { get; set; }

        public string  IdIngredient { get; set; }


        public string IdIngredientCategory { get; set; }
    }
}