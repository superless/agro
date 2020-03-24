using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.enums;

namespace trifenix.agro.db.model.agro {

    /// <summary>
    /// Producto Quimico usado por las órdenes
    /// </summary>
    [SharedCosmosCollection("agro", "Product")]
    public class Product : DocumentBaseName, ISharedCosmosEntity {

        public override string Id { get; set; }

        public override string Name { get; set; }

        public string IdActiveIngredient { get; set; }

        public string Brand { get; set; }

        public MeasureType MeasureType { get; set; }

        public double Quantity { get; set; }

        public KindOfProductContainer KindOfBottle { get; set; }

    }

}