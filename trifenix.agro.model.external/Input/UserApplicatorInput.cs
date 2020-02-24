using System;
using System.Collections.Generic;
using System.Text;

namespace trifenix.agro.model.external.Input
{
    public class UserApplicatorInput : InputBaseName
    {
        

        

        public string Rut { get; set; }

        public string Email { get; set; }

        public string IdJob { get; set; }

        public List<string> IdsRoles { get; set; }

        public string IdNebulizer { get; set; }

        public string IdTractor { get; set; }
    }
}
