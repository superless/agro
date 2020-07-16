using System.Collections.Generic;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.mdm.ts_model
{
    /// <summary>
    /// Metadata de una entidad o clase.
    /// </summary>
    public class EntityMetadata {

        /// <summary>
        /// Nombre de la entidad a mostrar
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// Nombre Corto de la entidad
        /// </summary>
        public string ShortName { get; set; }


        /// <summary>
        /// Descripción de la entidad
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Índice de la entidad
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Determina si la clase es visible
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Tipo de entidad, si es de tipo entity, se podría indicar que es común,
        /// por tanto puede ser generada automáticamente.
        /// </summary>
        public EntityKind EntityKind { get; set; }

        /// <summary>
        /// cadena de texto para la ruta de la entidad,
        /// esto puede ser usado para el react router 
        /// o las rutas de la api.
        /// </summary>
        public string PathName { get; set; }


        /// <summary>
        /// Autonumérico
        /// </summary>
        public bool AutoNumeric { get; set; }

        /// <summary>
        /// Nombre de la clase
        /// </summary>
        public string ClassName { get; set; }


        /// <summary>
        /// Metadata de las propiedades de tipo string
        /// </summary>
        public Dictionary<int, PropertyMetadata> StringData { get; set; }

        /// <summary>
        /// Metadata de las propiedades de tipo número.
        /// </summary>
        public Dictionary<int, PropertyMetadata> NumData { get; set; }

        /// <summary>
        /// Metadata de las propiedades de tipo double.
        /// </summary>
        public Dictionary<int, PropertyMetadata> DoubleData { get; set; }


        /// <summary>
        /// Metada de las propiedades de tipo boolean
        /// </summary>
        public Dictionary<int, PropertyMetadata> BoolData { get; set; }


        /// <summary>
        /// Metadata de las propiedades de tipo geo.
        /// </summary>
        public Dictionary<int, PropertyMetadata> GeoData { get; set; }

        /// <summary>
        /// Metadata de las propiedades de tipo fecha.
        /// </summary>
        public Dictionary<int, PropertyMetadata> DateData { get; set; }

        /// <summary>
        /// propiedades de tipo enumeración.
        /// </summary>
        public Dictionary<int, PropertyMetadadataEnum> EnumData { get; set; }

        /// <summary>
        /// propiedades de tipo entidad.
        /// </summary>
        public Dictionary<int, PropertyMetadata> relData { get; set; }

    }

   

    


}
