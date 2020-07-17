
using Cosmonaut;
using Cosmonaut.Attributes;
using System.Collections.Generic;
using trifenix.agro.db;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.agro.model
{

    /// <summary>
    /// Usuario
    /// </summary>
    [SharedCosmosCollection("agro", "User")]
    [ReferenceSearchHeader(EntityRelated.USER, PathName = "users", Kind = EntityKind.ENTITY)]
    public class User : DocumentBaseName<long>, ISharedCosmosEntity
    {


        /// <summary>
        /// identificador
        /// </summary>
        public override string Id { get; set; }


        // clave que vería el cliente.
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
