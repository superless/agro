using System;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.mdm_attributes
{
    /// <summary>
    /// Atributo para indicar que una propiedad es de tipo entidad.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ReferenceSearchAttribute : EntityIndexRelatedPropertyAttribute
    {
        /// <summary>
        /// Determina si la propiedad es de tipo entidad
        /// </summary>
        /// <param name="index">Índice que representa la propiedad</param>
        /// <param name="local">Si es local, significa que no tiene su propio key value en la base de dato de persistencia.</param>
        /// <param name="visible">Determina si la propiedad es visible en el cliente.</param>
        public ReferenceSearchAttribute(EntityRelated index, bool local = false, bool visible = true)
        {
            KindIndex = (int)(local ? KindEntityProperty.LOCAL_REFERENCE : KindEntityProperty.REFERENCE);
            Index = (int)index;
            Visible = visible;
            IsEntity = true;
        }

        public ReferenceSearchAttribute(EntityRelated index, EntityRelated bypass, bool visible = true)
        {
            KindIndex = (int)KindEntityProperty.REFERENCE;
            Index = (int)index;
            Visible = visible;
            RealIndex = (int)bypass;
            IsEntity = true;
        }



    }


}