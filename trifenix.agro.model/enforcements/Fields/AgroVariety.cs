using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model.enforcements.Fields
{

    [SharedCosmosCollection("agro", "AgroVariety")]
    public class AgroVariety : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public AgroSpecie Specie { get; set; }




    }


}
