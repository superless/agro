using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "PlotLand")]
    [ReferenceSearch(EntityRelated.PLOTLAND)]
    public class PlotLand : DocumentBaseName, ISharedCosmosEntity
    {
        public override string Id { get; set; }


        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [ReferenceSearch(EntityRelated.SECTOR)]
        public string IdSector { get; set; }

    }
}
