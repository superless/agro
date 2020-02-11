using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.userActivity.interfaces.model;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "Role")]
    public class Role : DocumentBase, ISharedCosmosEntity, IRole
    {
        public override string Id { get; set; }

        public string Name { get; set; }

    }
}
