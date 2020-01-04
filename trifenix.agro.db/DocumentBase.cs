using Cosmonaut.Attributes;
namespace trifenix.agro.db {
    public abstract class DocumentBase {
        public abstract string Id { get; set; }

        protected DocumentBase() {
            CosmosEntityName = GetType().Name;
        }

        [CosmosPartitionKey]
        public string CosmosEntityName { get; set; }

    }
}