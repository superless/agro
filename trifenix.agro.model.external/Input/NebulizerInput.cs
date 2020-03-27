using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input {
    public class NebulizerInput : InputBase {

        [Required]
        public string Brand { get; set; }

        [Required, UniqueAttribute]
        public string Code { get; set; }

    }

    public class NebulizerSwaggerInput {
        public string Brand { get; set; }
        public string Code { get; set; }
    }

}