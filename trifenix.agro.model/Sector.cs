using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "Sector")]
    public class Sector : DocumentBase, ISharedCosmosEntity {
        public override string Id { get; set; }
        public string SeasonId { get; set; }
        public string Name { get; set; }
    }

}