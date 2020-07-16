using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.db;

namespace trifenix.connect.agro.model
{


    // TODO : Cristian, documentar.
    [SharedCosmosCollection("agro", "EntityContainer")]
    public class EntityContainer : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }
        public dynamic Entity { get; set; }

    }

}