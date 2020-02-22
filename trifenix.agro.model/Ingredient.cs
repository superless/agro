using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model {
    [SharedCosmosCollection("agro", "Ingredient")]
    public class Ingredient : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }
        public string Name { get; set; }
        public IngredientCategory Category { get; set; }

    }
}