using System;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.mdm_attributes
{
    /// <summary>
    /// Propiedad que permite identificar de que tipo es una propiedad
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PropertyIndexAttribute : BaseIndexAttribute
    {
        /// <summary>
        /// declara la clase base como no entidad,
        /// la clase padre es usada para identificar todas las propiedades.
        /// de entidad o de el resto de tipo de datos.
        /// </summary>
        public PropertyIndexAttribute()
        {
            IsEntity = false;
        }

        /// <summary>
        /// asigna el tipo de propiedad al elemento padre.
        /// </summary>
        public KindProperty Related
        {
            get { return (KindProperty)KindIndex; }
            set { KindIndex = (int)value; }
        }


    }




}
