
using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

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
