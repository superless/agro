using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model.agro {

    [SharedCosmosCollection("agro", "Specie")]
    public class Specie : DocumentBase, ISharedCosmosEntity {
        public override string Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }
    }

    public enum Species {
        CI,     //Ciruela
        DU,     //Durazno
        KI,     //Kiwi
        NE,     //Nectarin
        CE,     //Cereza
        MANZA   //Manzana
    }

}
