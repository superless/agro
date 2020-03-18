using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input {
    public class TractorInput : InputBase {

        [Required]
        public string Brand { get; set; }

        [Required, Unique]
        public string Code { get; set; }

    }

    public class TractorSwaggerInput {

        public string Brand { get; set; }

        public string Code { get; set; }

    }

}