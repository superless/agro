using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model.enforcements.products
{
    /// <summary>
    /// Region where will be import the fruits.
    /// </summary>
    [SharedCosmosCollection("agro", "CertifierRegion")]
    public class CertifierRegion : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }

        /// <summary>
        /// Name of Certified Region
        /// </summary>
        public string Name { get; set; }
    }

}
