using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model
{
    [SharedCosmosCollection("agro", "Variety")]
    [ReferenceSearchHeader(EntityRelated.VARIETY, Kind = EntityKind.ENTITY, PathName = "varieties")]
    [ReferenceSearchHeader(EntityRelated.POLLINATOR, Kind = EntityKind.ENTITY, PathName = "pollinators")]
    public class Variety : DocumentBaseName<long>, ISharedCosmosEntity
    {
    
        public override string Id { get; set; }




        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }


        [ReferenceSearch(EntityRelated.SPECIE)]
        public string IdSpecie { get; set; }

        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }
    }
}
