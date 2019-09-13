using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model.enforcements.products
{
    [SharedCosmosCollection("agro", "ActiveIngredientCategory")]
    public class ActiveIngredientCategory : DocumentBase, ISharedCosmosEntity
    {

        public override string Id { get; set; }

        public string Name { get; set; }


    }

}
