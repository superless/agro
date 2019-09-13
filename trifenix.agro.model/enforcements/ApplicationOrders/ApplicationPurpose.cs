using Cosmonaut;
using Cosmonaut.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.db.model.enforcements.ApplicationOrders
{
    [SharedCosmosCollection("agro", "ApplicationPurpose")]
    public class ApplicationPurpose : DocumentBase, ISharedCosmosEntity
    {

        
        public override string Id { get; set; }
      
        public string Name { get; set; }
    }
}
