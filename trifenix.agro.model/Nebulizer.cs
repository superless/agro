using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "Nebulizer")]
    [ReferenceSearchHeader(EntityRelated.NEBULIZER, PathName ="nebulizers", Kind = EntityKind.ENTITY)]
    public class Nebulizer : DocumentBase<long>, ISharedCosmosEntity {

        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual de la entidad certificadora
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }


        [StringSearch(StringRelated.GENERIC_BRAND)]
        public string Brand { get; set; }

        [StringSearch(StringRelated.GENERIC_CODE)]
        public string Code { get; set; }

    }
}