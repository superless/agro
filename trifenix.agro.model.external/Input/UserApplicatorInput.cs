using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.db.model;


namespace trifenix.agro.model.external.Input {
    public class UserApplicatorInput : InputBase {

        [Required, Unique]
        public string Name { get; set; }

        [Required, Unique]
        public string Rut { get; set; }

        [UniqueAttribute]
        public string Email { get; set; }

        [ReferenceAttribute(typeof(Job))]
        public string IdJob { get; set; }

        [Required, ReferenceAttribute(typeof(Role))]
        public List<string> IdsRoles { get; set; }

        [ReferenceAttribute(typeof(Nebulizer))]
        public string IdNebulizer { get; set; }

        [ReferenceAttribute(typeof(Tractor))]
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