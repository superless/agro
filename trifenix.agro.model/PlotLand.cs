using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "PlotLand")]
    public class PlotLand : DocumentBase, ISharedCosmosEntity {
        public override string Id { get; set; }
        public string SeasonId { get; set; }
        public string Name { get; set; }
        public Sector Sector { get; set; }
    }
}