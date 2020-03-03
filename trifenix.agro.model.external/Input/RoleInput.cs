using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input
{
    public class RoleInput : InputBaseName
    {
      
    }

    public class RoleSwaggerInput {

        [Required]
        public string Name { get; set; }
    }
}
