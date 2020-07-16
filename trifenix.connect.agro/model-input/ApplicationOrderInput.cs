using System;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.model.external.Input;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro.model;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.model_input {

    [ReferenceSearchHeader(EntityRelated.ORDER)]
    public class ApplicationOrderInput : InputBase {

        [Required, Unique]
        [SuggestSearch(StringRelated.GENERIC_NAME)]
        public string Name { get; set; }

        [Required]
        [EnumSearch(EnumRelated.ORDER_TYPE)]
        public OrderType OrderType { get; set; }


        [Required]
        [DateSearch(DateRelated.START_DATE_ORDER)]
        public DateTime StartDate { get; set; }

        [Required]
        [DateSearch(DateRelated.END_DATE_ORDER)]
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