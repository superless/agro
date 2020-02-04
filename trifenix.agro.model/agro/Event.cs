using Cosmonaut;
using Cosmonaut.Attributes;
using System;

namespace trifenix.agro.db.model.agro {
    [SharedCosmosCollection("agro", "Event")]
    public class Event : DocumentBase, ISharedCosmosEntity {
        public override string Id { get; set; }
        public string Name { get; set; }
        public bool isPhenological { get; set; }
        public DateTime InitDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}