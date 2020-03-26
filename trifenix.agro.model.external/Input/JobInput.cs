using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input {
    public class JobInput : InputBaseName { }

    public class JobSwaggerInput {

        [Required]
        public string Name { get; set; }

    }

}