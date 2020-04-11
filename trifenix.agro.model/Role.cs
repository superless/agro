using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model
{

    [SharedCosmosCollection("agro", "Role")]
    [ReferenceSearch(EntityRelated.ROLE)]
    public class Role : DocumentBaseName, ISharedCosmosEntity
    {
        public override string Id { get; set; }


        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

    }
}
