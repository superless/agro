using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "Ingredient")]
    [ReferenceSearch(EntityRelated.INGREDIENT)]
    public class Ingredient : DocumentBaseName, ISharedCosmosEntity {

        public override string Id { get; set; }

        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [ReferenceSearch(EntityRelated.CATEGORY_INGREDIENT)]
        public string idCategory { get; set; }

    }
}