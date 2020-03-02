using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "OrderFolder")]
    public class OrderFolder : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }
        public string IdPhenologicalEvent { get; set; }
        public string IdApplicationTarget { get; set; }
        public string IdSpecie { get; set; }
        public string  IdIngredient { get; set; }
        public string IdCategoryIngredient { get; set; }
    }
}