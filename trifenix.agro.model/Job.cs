using Cosmonaut;
using Cosmonaut.Attributes;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model
{

    [SharedCosmosCollection("agro", "Job")]
    [ReferenceSearchHeader(EntityRelated.JOB, PathName = "jobs", Kind = EntityKind.ENTITY)]
    public class Job : DocumentBaseName<long>, ISharedCosmosEntity
    {
        /// <summary>
        /// Identificador visual de la entidad certificadora
        /// </summary>
        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }

        public override string Id { get; set; }


        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }

    }
}
