using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model.agro {

    [SharedCosmosCollection("agro", "Ingredient")]
    public class Ingredient : DocumentBaseName, ISharedCosmosEntity {

        public override string Id { get; set; }
        public override string Name { get; set; }
        public string idCategory { get; set; }

    }
}