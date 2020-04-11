using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "Nebulizer")]
    [ReferenceSearch(EntityRelated.NEBULIZER)]
    public class Nebulizer : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }

        [StringSearch(StringRelated.GENERIC_BRAND)]
        public string Brand { get; set; }

        [StringSearch(StringRelated.GENERIC_CODE)]
        public string Code { get; set; }

    }
}