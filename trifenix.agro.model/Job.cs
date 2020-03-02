using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "Job")]
    public class Job : DocumentBaseName, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public override string Name { get; set; }

    }
}
