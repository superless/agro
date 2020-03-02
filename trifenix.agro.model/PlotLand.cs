using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "PlotLand")]
    public class PlotLand : DocumentBaseName, ISharedCosmosEntity
    {
        public override string Id { get; set; }
        public override string Name { get; set; }
        public string IdSector { get; set; }

    }
}
