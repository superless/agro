using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.agro {

    [SharedCosmosCollection("agro", "ApplicationTarget")]
    [ReferenceSearch(EntityRelated.TARGET)]
    public class ApplicationTarget : DocumentBaseName, ISharedCosmosEntity {

        public override string Id { get; set; }

        [SuggestSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [StringSearch(StringRelated.GENERIC_ABBREVIATION)]
        public string Abbreviation { get; set; }

    }

}