using System.Collections.Generic;

namespace trifenix.connect.mdm.ts_model
{
    /// <summary>
    /// Metadata de una propiedad de tipo enumeración,
    /// esta hereda de la metadata de las otras entidades.
    /// </summary>
    public class PropertyMetadadataEnum : PropertyMetadata
    {   
        /// <summary>
        /// Descripción de cada valor del metadata.
        /// </summary>
        public Dictionary<int, string> EnumData { get; set; }
        
    }

   

    


}
