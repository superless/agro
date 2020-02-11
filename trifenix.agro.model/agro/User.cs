using Cosmonaut;
using Cosmonaut.Attributes;
using System.Collections.Generic;
using trifenix.userActivity.interfaces.model;

namespace trifenix.agro.db.model.agro
{
    [SharedCosmosCollection("agro", "User")]
    public class User : DocumentBase, ISharedCosmosEntity, IUser
    {
    
        public override string Id { get; set; }

        public string ObjectIdAAD { get; set; }

        public string Name { get; set; }

        public string Rut { get; set; }

        public string Email { get; set; }

        public IJob Job { get; set; }

        public List<IRole> Roles { get; set; }

    }
}
