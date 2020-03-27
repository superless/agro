using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input {
    public abstract class InputBase {
        public string Id { get; set; }

    }

    public abstract class InputBaseName : InputBase {
        [Required, UniqueAttribute]
        public string Name { get; set; }
    }
}