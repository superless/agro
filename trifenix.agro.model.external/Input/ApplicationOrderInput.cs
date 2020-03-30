using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.db.model;
using trifenix.agro.db.model.orders;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

    public class ApplicationOrderInput : InputBase {

        [Required, Unique]
        public string Name { get; set; }

        [Required]
        [EnumSearch(EnumRelated.ORDER_TYPE)]
        public OrderType OrderType { get; set; }


        [Required]
        [DateSearch(DateRelated.START_DATE)]
        public DateTime StartDate { get; set; }

        [Required]
        [DateSearch(DateRelated.END_DATE)]
        public DateTime EndDate { get; set; }

        [Required]
        [DoubleSearch(DoubleRelated.WETTING)]
        public double Wetting { get; set; }

        [Required]
        [ReferenceSearch(EntityRelated.DOSES, true)]
        public DosesOrder[] DosesOrder { get; set; }


        [Reference(typeof(PreOrder))]
        [ReferenceSearch(EntityRelated.PREORDER)]
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
        public DateTime StartDate { get; set; }

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