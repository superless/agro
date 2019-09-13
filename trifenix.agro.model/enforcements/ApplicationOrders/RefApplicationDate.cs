using Cosmonaut;
using Cosmonaut.Attributes;
using System;

namespace trifenix.agro.db.model.enforcements.ApplicationOrders
{
    [SharedCosmosCollection("agro", "RefApplicationDate")]
    public class RefApplicationDate : ReferenceApplicationOrder, ISharedCosmosEntity
    {
        public DateTime DateInit { get; set; }
    }
}
