using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.core {

    [SharedCosmosCollection("agro", "CostCenter")]
    [ReferenceSearchHeader(EntityRelated.COSTCENTER, PathName = "cost-centers", Kind = EntityKind.ENTITY)]
    public class CostCenter : DocumentBaseName<long>, ISharedCosmosEntity {

        public override string Id { get; set; }

        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }
        

        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [ReferenceSearch(EntityRelated.BUSINESSNAME)]
        public string IdBusinessName { get; set; }
        
    }

}