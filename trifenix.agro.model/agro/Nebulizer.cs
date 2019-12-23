using Cosmonaut;
using Cosmonaut.Attributes;
namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "Nebulizer")]
    public class Nebulizer : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public string Brand { get; set; }

        public string Code { get; set; }

    }
}
