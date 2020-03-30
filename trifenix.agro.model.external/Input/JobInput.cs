using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input {
    public class JobInput : InputBase {

        [Required, Unique]
        public string Name { get; set; }
    }

    public class JobSwaggerInput {

        [Required]
        public string Name { get; set; }

    }

}