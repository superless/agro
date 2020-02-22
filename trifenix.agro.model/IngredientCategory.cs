using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "IngredientCategory")]
    public class IngredientCategory : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }
        public string Name { get; set; }

    }
}