using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using trifenix.agro.attr;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.model.external.Input {
    public abstract class InputBase {

        [JsonIgnore]
        public string Id { get; set; }

    }

    
}