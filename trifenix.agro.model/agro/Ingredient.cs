using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.db.model.agro
{
    [SharedCosmosCollection("agro", "Ingredient")]
    public class Ingredient : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        public string Name { get; set; }

        public IngredientCategory Category { get; set; }

        



    }
}
