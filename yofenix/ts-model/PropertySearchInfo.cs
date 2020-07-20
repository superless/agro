using System.Collections.Generic;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.mdm.ts_model
{
    /// <summary>
    /// Información de una propiedad
    /// </summary>
    public class PropertySearchInfo
    {


        /// <summary>
        /// Índice de la propiedad.
        /// </summary>
        public int Index { get; set; }


        public bool IsEntity { get; set; }


        /// <summary>
        /// tipo de propiedad.
        /// </summary>
        public KindProperty Related { get; set; }


        /// <summary>
        /// tipo de propiedad.
        /// </summary>
        public KindEntityProperty RelatedEntity { get; set; }


        /// <summary>
        /// Nombre de la propiedad
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// índice de la clase a la que pertenece la propiedad.
        /// </summary>
        public int IndexClass { get; set; }

        /// <summary>
        /// Valores de una enumeración, el número y la descripción.
        /// </summary>
        public Dictionary<int, string> Enums { get; set; }


        /// <summary>
        /// índica si es un enumerable o no.
        /// </summary>
        public bool IsEnumerable { get; set; }

        /// <summary>
        /// Es requerido. 
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Es único.
        /// </summary>
        public bool IsUnique { get; set; }

        /// <summary>
        /// descripción de la propiedad
        /// </summary>
        public EntitySearchDisplayInfo Info { get; set; }

        /// <summary>
        /// Si la propiedad es visible.
        /// </summary>
        public bool Visible { get; set; }


        /// <summary>
        /// Si es un autonumérico.
        /// </summary>
        public bool AutoNumeric { get; set; }

        /// <summary>
        /// Determina si tiene entrada (Existe una clase con esa propiedad en el namespace de entradas).
        /// </summary>
        public bool HasInput { get; set; }


        /// <summary>
        /// Ancho máximo de la propiedad.
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// Ancho Mínimo de la propiedad.
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// Grupo al que pertenece. 
        /// </summary>
        public GroupInput[] Group { get; set; }






    }
}
