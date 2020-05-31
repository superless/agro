using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model
{

    [SharedCosmosCollection("agro", "IngredientCategory")]
    [ReferenceSearchHeader(EntityRelated.CATEGORY_INGREDIENT, PathName = "ingredient_categories", Kind = EntityKind.ENTITY)]
    public class IngredientCategory : DocumentBaseName, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }


    }
}
