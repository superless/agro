using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.model.enforcements.products
{
    /// <summary>
    /// Represent the active ingredient used by a Phitosanitary Product.
    /// </summary>
    public class ActiveIngredient
    {
        public string Id { get; set; }

        /// <summary>
        /// Name of the active ingredient
        /// </summary>
        public string Name { get; set; }

    }

}
