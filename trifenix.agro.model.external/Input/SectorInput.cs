using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.model.external.Input {
    public class SectorInput : InputBase {
        [Required, Unique]
        public string Name { get; set; }
    }

    /// <summary>
    /// Para ingresar un Sector debe determinar solo el nombre.
    /// </summary>
    public class SectorSwaggerInput {
        
        [Required]
        public string Name { get; set; }

    }
    
}