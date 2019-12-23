using Cosmonaut;
using Cosmonaut.Attributes;
namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "Tractor")]
    public class Tractor : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public string Brand { get; set; }

        public string Code { get; set; }

    }
}
