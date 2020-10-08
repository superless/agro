using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.connect.entities.cosmos
{


    // TODO : Cristian, documentar.
    [SharedCosmosCollection("agro", "EntityContainer")]
    public class EntityContainer : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }

        
        public override string ClientId { get; set; }

        public dynamic Entity { get; set; }

    }

}