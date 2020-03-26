using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model.agro
{

    [SharedCosmosCollection("agro", "IngredientCategory")]
    [ReferenceSearch(EntityRelated.CATEGORY_INGREDIENT)]
    public class IngredientCategory : DocumentBaseName, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }


    }
}
