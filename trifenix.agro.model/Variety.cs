using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model {
    [SharedCosmosCollection("agro", "Variety")]
    public class Variety : DocumentBase, ISharedCosmosEntity {
    
        public override string Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public Specie Specie { get; set; }

    }
}