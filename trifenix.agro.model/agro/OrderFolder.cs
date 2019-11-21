using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.db.model.agro.local;


namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "OrderFolder")]
    public class OrderFolder : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public PhenologicalEvent PhenologicalEvent { get; set; }

        public ApplicationTarget ApplicationTarget { get; set; }


        public Specie Specie { get; set; }

        public Rootstock Rootstock { get; set; }

        public IngredientCategory Category { get; set; }

        public LocalIngredient Ingredient { get; set; }

        public PhenologicalStage Stage { get; set; }

        public string SeasonId { get; set; }











    }
}
