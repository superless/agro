using System;
using trifenix.connect.mdm.enums;


namespace trifenix.connect.mdm_attributes
{
    /// <summary>
    /// Atributo que identifica una propiedad con una entidad relacionada.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EntityIndexRelatedPropertyAttribute : BaseIndexAttribute
    {

         ///<summary>
        /// declara la clase base como entidad,
        /// la clase padre es usada para identificar todas las propiedades.
        /// de entidad o de el resto de tipo de datos.
        /// </summary>
        public EntityIndexRelatedPropertyAttribute()
        {
            IsEntity = true;
        }

        /// <summary>
        /// Se usa exclusivamente cuando una entidad apunto a otra entidad en más de una propiedad.
        /// el modelo no puede soportar dos propiedades que apuntan a la misma entidad.
        /// cuando se da el caso, se debe crear un bypass, por ejemplo.
        /// Un cuartel apunta a variedad, a través de la propiedad variedad, pero también en polinizador.
        /// el modelo mdm cuando detecta que existen varias propiedades que apuntan a la misma entidad
        /// los agrupa y los lleva como un array de entidades relacionadas, con el índice y su valor, no diferenciando que sean propiedades distintas.
        /// Para poder lograr que esto no pase, en vez de usar el índice de variedad, se usa el índice de otra clase, en este caso polinizador.
        /// declarandolo como una referencia hacia Variedades, la clase polinizador puede ser vacia.
        /// Siempre que se consulte sobre polinizador se sabrá que es solo una referencia 
        /// y se apuntará al que corresponde realmente.
        /// !Importante, esto no afecta el modelo de clases, el modelo de clases se mantiene igual, solo se debe considerar a nivel de los atributos MDM 
        /// y crear una clase que haga la referencia, como en el ejemplo Polinator.
        /// 
        /// </summary>
        public int Bypass { get; set; } = -1;

    }


    /// <summary>
    /// Determina el pathname que será usado en la clase y
    /// el tipo de entidad.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EntityIndexAttribute : BaseIndexAttribute {

        /// <summary>
        /// determina un pathname que puede ser usado en el router del cliente
        /// o en la url de la api rest.
        /// </summary>
        public string PathName { get; set; }


        /// <summary>
        /// Determina el tipo de entidad, 
        /// Si es de tipo entity, significa que es lo suficientemente simple para generar su formulario
        /// de manera automática, 
        /// si es de´tipo proceso, se identificará como documento o proceso.
        /// y si es custom, se entenderá que puede no ser un proceso, pero debe ser construido manualmente.
        /// </summary>
        public EntityKind Kind { get; set; }


        


    }





}
