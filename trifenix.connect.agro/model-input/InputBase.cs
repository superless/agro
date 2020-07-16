using Newtonsoft.Json;



namespace trifenix.connect.agro.model_input
{
    public abstract class InputBase {

        [JsonIgnore]
        public string Id { get; set; }

    }

    
}