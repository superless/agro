using Cosmonaut.Attributes;
using trifenix.connect.model;

namespace trifenix.connect.entities.cosmos
{
    public abstract class DocumentBase : DocumentDb {


        [CosmosPartitionKey]
        public string CosmosEntityName { get; set; }

        protected DocumentBase() {
            CosmosEntityName = GetType().Name;
        }

    }

}