using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "Specie")]
    public class Specie : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public string Name { get; set; }


        public string Abbreviation { get; set; }

    }
}
