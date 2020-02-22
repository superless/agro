using Cosmonaut;
using Cosmonaut.Attributes;
using System.Collections.Generic;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "Counter")]
    public class Counter : DocumentBase, ISharedCosmosEntity {

        public override string Id { get; set; }

        //{ApplicationOrder:{CI:5,DU:10...}}
        public Dictionary<string, Dictionary<string,int>> Count { get; set; }

    }
}