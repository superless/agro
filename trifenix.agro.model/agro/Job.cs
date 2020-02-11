using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.userActivity.interfaces.model;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "Job")]
    public class Job : DocumentBase, ISharedCosmosEntity, IJob
    {
        public override string Id { get; set; }

        public string Name { get; set; }

    }
}
