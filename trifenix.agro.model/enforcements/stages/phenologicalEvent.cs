
using Cosmonaut;
using Cosmonaut.Attributes;
using System;

namespace trifenix.agro.db.model.enforcements.stages
{
    [SharedCosmosCollection("agro", "PhenologicalEvent")]
    public class PhenologicalEvent : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }
        public string Name { get; set; }
        public DateTime InitDate { get; set; }

        


    }
}
