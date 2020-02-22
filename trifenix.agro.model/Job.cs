using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model {
    [SharedCosmosCollection("agro", "Job")]
    public class Job : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }
        public string Name { get; set; }

    }
}