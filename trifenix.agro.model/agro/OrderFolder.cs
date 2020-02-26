using Cosmonaut;
using Cosmonaut.Attributes;
using System.Collections.Generic;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.enums;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "OrderFolder")]
    public class OrderFolder : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public string IdPhenologicalEvent { get; set; }

        

        

        public string IdApplicationTarget { get; set; }

        public string IdSpecie { get; set; }

        public Rootstock Rootstock { get; set; }

        public IngredientCategory Category { get; set; }

        public LocalIngredient Ingredient { get; set; }

        public PhenologicalStage Stage { get; set; }

        public string SeasonId { get; set; }











    }
}
