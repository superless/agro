using trifenix.agro.enums;

namespace trifenix.agro.model.external.Input
{
    public class ProductInput : InputBaseName
    {
        

        public string IdActiveIngredient { get; set; }

        public string Brand { get; set; }

        public MeasureType MeasureType { get; set; }

        public double Quantity { get; set; }

        public KindOfProductContainer KindOfBottle { get; set; }

        public DosesInput[] Doses { get; set; }






    }
}
