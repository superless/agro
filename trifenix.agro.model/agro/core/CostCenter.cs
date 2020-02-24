using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model.agro.core
{

    [SharedCosmosCollection("agro", "CostCenter")]
    public class CostCenter : DocumentBaseName, ISharedCosmosEntity {

        public override string Id { get; set; }
        public override string Name { get; set; }
        public string IdBusinessName { get; set; }
        

    }
}