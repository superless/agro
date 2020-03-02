using Cosmonaut;
using Cosmonaut.Attributes;
using System.Collections.Generic;

namespace trifenix.agro.db.model {
    [SharedCosmosCollection("agro", "User")]
    public class User : DocumentBaseName, ISharedCosmosEntity {
    
        public override string Id { get; set; }

        public string ObjectIdAAD { get; set; }

        public override string Name { get; set; }

        public string Rut { get; set; }

        public string Email { get; set; }

        public string IdJob { get; set; }

        public List<string> IdsRoles { get; set; }

    }
}