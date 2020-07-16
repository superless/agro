using System;

using System.Text;

namespace trifenix.connect.mdm_attributes
{

    /// <summary>
    /// Atributo principal de mdm, desde este atributo heredarán otros más específicos, a través de esta base se podrá convertir una clase a un entitySearch y viceversa,
    /// los índices permitirán identificar las propiedades de una clase y asociarlas con el entitySearch.  
    /// 
    /// </summary>
    public class BaseIndexAttribute : Attribute
    {
        /// <summary>
        /// Índice de la propiedad.
        /// </summary>
        public int Index { get;  set; }

        /// <summary>
        /// determina si la propiedad es visible.
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// determina si la entidad es una entidad.
        /// </summary>
        public bool IsEntity { get; protected set; }

        /// <summary>
        /// índice del tipo de  propiedad
        /// tendrá distintos indices si es entidad u propiedad.
        /// </summary>
        public int KindIndex { get; set; }
    }





}
