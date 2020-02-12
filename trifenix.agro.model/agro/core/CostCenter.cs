using Cosmonaut;
using Cosmonaut.Attributes;
using System;

namespace trifenix.agro.db.model.agro.core {

    [SharedCosmosCollection("agro", "CostCenter")]
    public class CostCenter : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }
        public string Name { get; set; }
        public string IdRazonSocial { get; set; }
        public DateTime CreatedAt { get; set; }
        public User LastModifier { get; set; }

    }
}