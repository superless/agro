using System;
using System.Collections.Generic;
using trifenix.userActivity.interfaces.model;

namespace trifenix.userActivity.model
{
    public class User : IUser
    {

        public string Id { get; set; }


        public string ObjectIdAAD { get; set; }

        public string Name { get; set; }

        
        public string Rut { get; set; }
        public string Email { get; set; }
        public IJob Job { get; set; }
        public List<IRole> Roles { get; set; }
    }

    public class Role : IRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Job : IJob
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }


}
