using Cosmonaut;
using Cosmonaut.Attributes;
using System;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "PhenologicalEvent")]
    public class PhenologicalEvent : DocumentBaseName, ISharedCosmosEntity {

        public override string Id { get; set; }
        public override string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }

}