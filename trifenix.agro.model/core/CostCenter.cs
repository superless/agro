using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.agro.core {

    [SharedCosmosCollection("agro", "CostCenter")]
    [ReferenceSearch(EntityRelated.COSTCENTER)]
    public class CostCenter : DocumentBaseName, ISharedCosmosEntity {

        public override string Id { get; set; }

        [SuggestSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [ReferenceSearch(EntityRelated.BUSINESSNAME)]
        public string IdBusinessName { get; set; }
        
    }

}