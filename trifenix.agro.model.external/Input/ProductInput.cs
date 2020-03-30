using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model;
using trifenix.agro.enums;
using trifenix.agro.enums.model;

namespace trifenix.agro.model.external.Input {
    public class ProductInput : InputBase {

        [Required, Unique]
        public string Name { get; set; }

        [Required, ReferenceAttribute(typeof(Ingredient))]
        public string IdActiveIngredient { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public MeasureType MeasureType { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public KindOfProductContainer KindOfBottle { get; set; }

        [Required]
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

        [Required]
        public DosesInput[] Doses { get; set; }

    }

}