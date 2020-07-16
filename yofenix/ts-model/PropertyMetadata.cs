

namespace trifenix.connect.mdm.ts_model
{
    /// <summary>
    /// Metadata de una propiedad.
    /// </summary>
    public class PropertyMetadata
    {

        /// <summary>
        /// Nombre de la propiedad
        /// </summary>
        public string NameProp { get; set; }

        
        /// <summary>
        /// determina si es o no un array.
        /// </summary>
        public bool isArray { get; set; }

        /// <summary>
        /// Metadata para ser usada por el html.
        /// </summary>
        public EntitySearchDisplayInfo Info { get; set; }

        /// <summary>
        /// Determina si la propiedad es requerida.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Determina si la propiedad es única,
        /// si lo es, solo puede existir solo una propiedad con ese nombre en la colección de entidades.
        /// </summary>
        public bool Unique { get; set; }

        /// <summary>
        /// Determina si la propiedad es visible en un formulario.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Determina si la propiedad es autonumeríca.
        /// </summary>
        public bool AutoNumeric { get; set; }


        /// <summary>
        /// Si la propiedad tiene una entrada (las clases de tipo entrada estan conectadas porl el ReferenceSearchHeader)
        /// </summary>
        public bool HasInput { get; set; }

    }

   

    


}
