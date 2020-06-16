using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "Tractor")]
    [ReferenceSearchHeader(EntityRelated.TRACTOR, Kind = EntityKind.ENTITY, PathName = "tractors")]
    public class Tractor : DocumentBase<long>, ISharedCosmosEntity {

        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }


        [StringSearch(StringRelated.GENERIC_BRAND)]
        public string Brand { get; set; }


        [StringSearch(StringRelated.GENERIC_CODE)]
        public string Code { get; set; }

    }

}