using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "Role")]
    public class Role : DocumentBaseName, ISharedCosmosEntity
    {
        public override string Id { get; set; }


        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

    }
}
