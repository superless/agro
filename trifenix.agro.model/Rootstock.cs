using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "Rootstock")]
    public class Rootstock : DocumentBase, ISharedCosmosEntity {
        public override string Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }
}