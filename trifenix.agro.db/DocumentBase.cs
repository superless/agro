using Cosmonaut.Attributes;
namespace trifenix.agro.db {
    public abstract class DocumentBase {

        public abstract string Id { get; set; }

        [CosmosPartitionKey]
        public string CosmosEntityName { get; set; }

        protected DocumentBase() {
            CosmosEntityName = GetType().Name;
        }

    }

    public abstract class DocumentBase<T> : DocumentBase
    {
        public abstract T ClientId { get; set; }
    }

    public abstract class DocumentBaseName<T> : DocumentBaseName
    {
        public abstract T ClientId { get; set; }
    }

    public abstract class DocumentBaseName : DocumentBase {

        public abstract string Name { get; set; }

    }

}