using System;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.indexes;
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
        /// <param name="index">índice que representa la propiedad</param>
        /// <param name="local">si es local, significa que no tiene su propio key value en la base de dato de persistencia.</param>
        /// <param name="visible">determina si la propiedad es visible en el cliente.</param>
        public ReferenceSearchAttribute(EntityRelated index, bool local = false, bool visible = true):base()
        {
            KindIndex = (int)(local ? KindEntityProperty.LOCAL_REFERENCE : KindEntityProperty.REFERENCE);
            Index = (int)index;
            Visible = visible;
        }

        public ReferenceSearchAttribute(EntityRelated index, EntityRelated bypass, bool local = false, bool visible = true) : base()
        {
            KindIndex = (int)(local ? KindEntityProperty.LOCAL_REFERENCE : KindEntityProperty.REFERENCE);
            Index = (int)index;
            Visible = visible;
            Bypass = (int)bypass;
        }



    }


}