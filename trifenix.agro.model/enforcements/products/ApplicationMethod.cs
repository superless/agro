using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model.enforcements.products
{

    [SharedCosmosCollection("agro", "ApplicationMethod")]
    public class ApplicationMethod : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }
        public string Name { get; set; }

    }
}