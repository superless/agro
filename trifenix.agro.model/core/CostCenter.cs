using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.core {

    [SharedCosmosCollection("agro", "CostCenter")]
    [ReferenceSearch(EntityRelated.COSTCENTER)]
    public class CostCenter : DocumentBaseName, ISharedCosmosEntity {

        public override string Id { get; set; }

        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [ReferenceSearch(EntityRelated.BUSINESSNAME)]
        public string IdBusinessName { get; set; }
        
    }

}