using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "Rootstock")]
    [ReferenceSearchHeader(EntityRelated.ROOTSTOCK, PathName = "rootstock", Kind = EntityKind.ENTITY)]
    public class Rootstock : DocumentBaseName<long>, ISharedCosmosEntity {

        public override string Id { get; set; }

        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }


        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }

        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }

    }
}