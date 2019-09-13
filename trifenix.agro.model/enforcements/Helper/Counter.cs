using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.db.model.enforcements.Helper
{

    [SharedCosmosCollection("agro", "Counter")]
    public class Counter : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public long NextTaskId { get; set; }




    }
}
