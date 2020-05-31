using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using trifenix.agro.attr;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model {
    [SharedCosmosCollection("agro", "Season")]
    [ReferenceSearchHeader(EntityRelated.SEASON, PathName = "seasons")]
    public class Season : DocumentBase, ISharedCosmosEntity {
        public override string Id { get; set; }

        [DateSearch(DateRelated.START_DATE_SEASON)]
        public DateTime StartDate { get; set; }

        [DateSearch(DateRelated.END_DATE_SEASON)]
        public DateTime EndDate { get; set; }

        [BoolSearch(BoolRelated.CURRENT)]
        public bool Current { get; set; }

        [ReferenceSearch(EntityRelated.COSTCENTER)]
        public string IdCostCenter { get; set; }

    }

}