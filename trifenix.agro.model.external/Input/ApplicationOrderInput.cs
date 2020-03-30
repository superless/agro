using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.db.model;
using trifenix.agro.db.model.orders;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {

    [ReferenceSearch(EntityRelated.ORDER)]
    public class ApplicationOrderInput : InputBase {

        [Required, Unique]
        [SuggestSearch(StringRelated.GENERIC_NAME)]
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
        [ReferenceSearch(EntityRelated.DOSES_ORDER, true)]
        public DosesOrder[] DosesOrder { get; set; }


        [Reference(typeof(PreOrder))]
        [ReferenceSearch(EntityRelated.PREORDER)]
        public string[] IdsPreOrder { get; set; }

        [Required]
        [ReferenceSearch(EntityRelated.BARRACK_EVENT, true)]
        public BarrackOrderInstance[] Barracks { get; set; }

    }


   

}