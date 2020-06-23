using Cosmonaut;
using Cosmonaut.Attributes;
using System.Collections.Generic;
using trifenix.agro.attr;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model {
    [SharedCosmosCollection("agro", "User")]
    [ReferenceSearchHeader(EntityRelated.USER, PathName ="users", Kind = EntityKind.ENTITY)]
    public class User : DocumentBaseName<long>, ISharedCosmosEntity {

      
        public override string Id { get; set; }


        [AutoNumericSearch(NumRelated.GENERIC_CORRELATIVE)]
        public override long ClientId { get; set; }

        public string ObjectIdAAD { get; set; }

        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }


        [StringSearch(StringRelated.GENERIC_RUT)]
        public string Rut { get; set; }

        [StringSearch(StringRelated.GENERIC_EMAIL)]
        public string Email { get; set; }

        [ReferenceSearch(EntityRelated.JOB)]
        public string IdJob { get; set; }

        [ReferenceSearch(EntityRelated.ROLE)]
        public List<string> IdsRoles { get; set; }

    }
}