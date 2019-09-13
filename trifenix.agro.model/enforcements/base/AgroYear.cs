using Cosmonaut;
using Cosmonaut.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.db.model.enforcements.@base
{
    [SharedCosmosCollection("agro", "AgroYear")]
    public class AgroYear : DocumentBase, ISharedCosmosEntity
    {
        
        public override string Id { get; set; }


        public DateTime Start { get; set; }


        public DateTime End { get; set; }


        public bool Current { get; set; }



    }
}
