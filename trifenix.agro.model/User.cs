using Cosmonaut;
using Cosmonaut.Attributes;
using System.Collections.Generic;
using trifenix.agro.attr;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.db.model {
    [SharedCosmosCollection("agro", "User")]
    [ReferenceSearch(EntityRelated.USER)]
    public class User : DocumentBaseName, ISharedCosmosEntity {
    

        public override string Id { get; set; }

        [StringSearch(StringRelated.OBJECTID_AAD)]
        public string ObjectIdAAD { get; set; }

        [StringSearch(StringRelated.GENERIC_NAME)]
        public override string Name { get; set; }


        [StringSearch(StringRelated.RUT)]
        public string Rut { get; set; }

        [StringSearch(StringRelated.EMAIL)]
        public string Email { get; set; }

        [ReferenceSearch(EntityRelated.JOB)]
        public string IdJob { get; set; }

        [ReferenceSearch(EntityRelated.ROLE)]
        public List<string> IdsRoles { get; set; }

    }
}