using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using trifenix.agro.attr;
using trifenix.agro.enums;

namespace trifenix.agro.db.model.agro {
    [SharedCosmosCollection("agro", "Season")]
    public class Season : DocumentBase, ISharedCosmosEntity {
        public override string Id { get; set; }


        [DateSearch(DateRelated.END_DATE)]
        public DateTime StartDate { get; set; }


        [DateSearch(DateRelated.END_DATE)]
        public DateTime EndDate { get; set; }

        [BoolSearch(BoolRelated.CURRENT)]
        public bool Current { get; set; }

        [ReferenceSearch(EntityRelated.COSTCENTER)]
        public string IdCostCenter { get; set; }

    }

}