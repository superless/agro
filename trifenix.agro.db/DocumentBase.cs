using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.db
{
    public abstract class DocumentBase
    {
        public abstract string Id { get; set; }

        public DocumentBase()
        {
            CosmosEntityName = GetType().Name;
        }

        [CosmosPartitionKey]
        public string CosmosEntityName { get; set; }
    }
}
