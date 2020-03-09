using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model.agro {

    [SharedCosmosCollection("agro", "EntityContainer")]
    public class EntityContainer : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }
        public dynamic Entity { get; set; }

    }

}