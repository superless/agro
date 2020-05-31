using Cosmonaut;
using Cosmonaut.Attributes;
using System;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model {

    [SharedCosmosCollection("agro", "PhenologicalEvent")]
    [ReferenceSearchHeader(EntityRelated.PHENOLOGICAL_EVENT, Kind = EntityKind.ENTITY, PathName = "phenological_events")]
    public class PhenologicalEvent : DocumentBaseName<long>, ISharedCosmosEntity {

        public override string Id { get; set; }


        /// <summary>
        /// Identificador visual 
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }


        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

        [DateSearch(DateRelated.START_DATE_PHENOLOGICAL_EVENT)]
        public DateTime StartDate { get; set; }

        [DateSearch(DateRelated.END_DATE_PHENOLOGICAL_EVENT)]
        public DateTime EndDate { get; set; }

    }
}