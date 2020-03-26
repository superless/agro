using System.ComponentModel.DataAnnotations;
using trifenix.agro.enums;
using trifenix.agro.enums.model;

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

    public class ProductSwaggerInput {

        [Required]
        public string Name { get; set; }

        [Required]
        public string IdActiveIngredient { get; set; }


        [Required]
        public string Brand { get; set; }

        [Required]
        public MeasureType MeasureType { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public KindOfProductContainer KindOfBottle { get; set; }

        
        public DosesSwaggerInput[] Doses { get; set; }
    }
}
