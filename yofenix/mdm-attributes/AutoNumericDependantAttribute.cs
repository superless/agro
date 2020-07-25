using System;
using trifenix.connect.mdm.enums;

namespace trifenix.connect.mdm_attributes
{
    /// <summary>
    /// Atributo que determina que un campo es autonumérico
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AutoNumericDependantAttribute : PropertyIndexAttribute
    {

        public AutoNumericDependantAttribute()
        {
            KindIndex = (int)KindProperty.NUM64;
        }

        /// <summary>
        /// la autonumeneración puede depnder de otra entidad, para comenzar desde el principio,
        /// por ejemplo, una dosis tendrá un autonumérico que depende de producto,
        /// el producto 1, tendría  la dosis 1, 2 y 3
        /// y el producto 2, tendría la dosis 1 y 2.
        /// </summary>
        public int? Dependant { get; set; }
    }


}
