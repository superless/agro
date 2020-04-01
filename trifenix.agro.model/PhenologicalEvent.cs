using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "PhenologicalEvent")]
    [ReferenceSearch(EntityRelated.PHENOLOGICAL_EVENT)]
    public class PhenologicalEvent : DocumentBaseName, ISharedCosmosEntity {

        public override string Id { get; set; }

        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [DateSearch(DateRelated.START_DATE_PHENOLOGICAL_EVENT)]
        public DateTime StartDate { get; set; }

        [DateSearch(DateRelated.END_DATE_PHENOLOGICAL_EVENT)]
        public DateTime EndDate { get; set; }

    }
}