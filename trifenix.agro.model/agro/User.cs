using Cosmonaut;
using Cosmonaut.Attributes;
using System.Collections.Generic;

namespace trifenix.agro.db.model.agro
{
    [SharedCosmosCollection("agro", "User")]
    public class User : DocumentBase, ISharedCosmosEntity
    {
    
        public override string Id { get; set; }

        public string ObjectIdAAD { get; set; }

        public string Name { get; set; }

        public string Rut { get; set; }

        public string Email { get; set; }

        public Job Job { get; set; }

        public List<Role> Roles { get; set; }

    }
}
