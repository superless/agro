using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model.enforcements.products
{

    /// <summary>
    /// Represent the active ingredient used by a Phitosanitary Product.
    /// </summary>
    [SharedCosmosCollection("agro", "ActiveIngredient")]
    public class ActiveIngredient : DocumentBase, ISharedCosmosEntity
    {

        /// <summary>
        /// Id from active ingredient
        /// </summary>
        public override string Id { get; set; }

        /// <summary>
        /// Name of the active ingredient
        /// </summary>
        public string Name { get; set; }

        public ActiveIngredientCategory Category { get; set; }

    }

}
