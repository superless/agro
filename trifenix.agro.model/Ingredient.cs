using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "Ingredient")]
    [ReferenceSearchHeader(EntityRelated.INGREDIENT, Kind = EntityKind.ENTITY, PathName = "ingredients")]
    public class Ingredient : DocumentBaseName<long>, ISharedCosmosEntity {

        public override string Id { get; set; }

        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }

        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [ReferenceSearch(EntityRelated.CATEGORY_INGREDIENT)]
        public string idCategory { get; set; }

    }
}