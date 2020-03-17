using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.enums;

namespace trifenix.agro.model.external.Input {

    public class ApplicationOrderInput : InputBaseName {

        [Required]
        public OrderType OrderType { get; set; }

        [Required]
        public DateTime InitDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public double Wetting { get; set; }

        [Required]
        public DosesOrder[] DosesOrder { get; set; }

        [Reference(typeof(PreOrder))]
        public string[] IdsPhenologicalPreOrder { get; set; }

        [Required]
        public BarrackOrderInstance[] Barracks { get; set; }

    }


    public class ApplicationOrderSwaggerInput {

        [Required]
        public string Name { get; set; }

        [Required]
        public OrderType OrderType { get; set; }

        [Required]
        public DateTime InitDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public double Wetting { get; set; }

        [Required]
        public DosesOrder[] DosesOrder { get; set; }

        public string[] IdsPhenologicalPreOrder { get; set; }

        [Required]
        public BarrackOrderInstance[] Barracks { get; set; }

    }

}