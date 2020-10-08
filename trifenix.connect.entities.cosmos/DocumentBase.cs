using Cosmonaut.Attributes;
namespace trifenix.connect.entities.cosmos
{
    public abstract class DocumentBase  {

        public abstract string Id { get; set; }
        public abstract string ClientId { get; set; }

        [CosmosPartitionKey]
        public string CosmosEntityName { get; set; }

        protected DocumentBase() {
            CosmosEntityName = GetType().Name;
        }

    }

}