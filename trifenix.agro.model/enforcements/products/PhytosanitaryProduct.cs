using Cosmonaut;
using Cosmonaut.Attributes;

namespace trifenix.agro.db.model.enforcements.products
{

    [SharedCosmosCollection("agro", "PhytosanitaryProduct")]
    public class PhytosanitaryProduct : DocumentBase, ISharedCosmosEntity
    {
        public override string Id { get; set; }
        public string CommercialName { get; set; }

        public ActiveIngredient ActiveIngredient { get; set; }

        public string PicturePath { get; set; }


    }


}
