using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "IngredientCategory")]
    public class IngredientCategory : DocumentBaseName, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public override string Name { get; set; }


    }
}
