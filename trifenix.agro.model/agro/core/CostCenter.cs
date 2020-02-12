using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.db.model.agro.local;

namespace trifenix.agro.db.model.agro.core {

    [SharedCosmosCollection("agro", "CostCenter")]
    public class CostCenter : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }
        public string Name { get; set; }
        public string IdReason { get; set; }
        public UserActivity Modify { get; set; }

    }
}