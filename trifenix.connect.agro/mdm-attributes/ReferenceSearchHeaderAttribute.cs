using System;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm_attributes;

namespace trifenix.connect.agro.mdm_attributes
{
    /// <summary>
    /// Atributo que determina la entidad que representa una clase.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ReferenceSearchHeaderAttribute : EntityIndexAttribute
    {
        /// <summary>
        /// Incluye la entidad en la metadata.
        /// </summary>
        /// <param name="index">Índice de una entidad</param>
        /// <param name="local">Si es local, significa que no tiene su propio key value en la base de dato de persistencia.</param>
        /// <param name="visible">Determina si la clase es visible en el cliente.</param>
        /// <param name="pathname">Ruta usada para la API o el router del cliente</param>
        /// <param name="entityKind">Determina si es una entidad, un proceso u otro.</param>
        public ReferenceSearchHeaderAttribute(EntityRelated index, bool local = false, bool visible = true, string pathname="", EntityKind entityKind = EntityKind.ENTITY)
        {
            KindIndex = (int)(local ? KindEntityProperty.LOCAL_REFERENCE : KindEntityProperty.REFERENCE);
            Index = (int)index;
            Visible = visible;
            PathName = pathname;
            Kind = entityKind;
        }


        



    }


}