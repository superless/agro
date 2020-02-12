using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.userActivity.interfaces.model;

namespace trifenix.agro.db.model.agro.core {

    [SharedCosmosCollection("agro", "CostCenter")]
    public class CostCenter : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }
        public string Name { get; set; }
        public string IdRazonSocial { get; set; }
        public IUserActivity Modify { get; set; }

    }
}