using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.model.external.Input {
    public class UserApplicatorInput : InputBaseName {

        [Required, Unique]
        public string Rut { get; set; }

        [Unique]
        public string Email { get; set; }

        [Reference(typeof(Job))]
        public string IdJob { get; set; }

        [Required, Reference(typeof(Role))]
        public List<string> IdsRoles { get; set; }

        [Reference(typeof(Nebulizer))]
        public string IdNebulizer { get; set; }

        [Reference(typeof(Tractor))]
        public string IdTractor { get; set; }
    }

    public class UserApplicatorSwaggerInput {

        [Required]
        public string Name { get; set; }

        public string Rut { get; set; }

        public string Email { get; set; }

        [Required]
        public string IdJob { get; set; }

        [Required]
        public List<string> IdsRoles { get; set; }

        public string IdNebulizer { get; set; }

        public string IdTractor { get; set; }

    }

}