using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input {
    public class RoleInput : InputBase {
        [Required, Unique]
        public string Name { get; set; }
    }

    public class RoleSwaggerInput {
        [Required]
        public string Name { get; set; }
    }

}