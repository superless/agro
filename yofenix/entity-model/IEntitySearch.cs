using System;

namespace trifenix.connect.mdm.entity_model
{
    /// <summary>
    /// Entidad base de fenix-mdm
    /// esta estructura permite guardar todas las entidades en agrupaciones por tipo de dato.
    /// el objetivo es generar este tipo de entidades desde el modelo de clases y viceversa,
    /// Puedo generar un entity search desde cualquier tabla o clase, la diferencia es que para poder convertir desde 
    /// el entity search, necesito la metadata, es decir, el diccionario que relaciona los índices de este modelo con los nombres
    /// de clases y propiedades.
    /// Este modelo permite a componentes webs, móviles o desktop realizar consultas a este modelo, 
    /// pudiendo generar una sólida arquitectura de componentes.
    /// el tipo de datos corresponde al tipo geo, que dependerá de la base de datos.
    /// <typeparamref name="T">Representa el campo geo que su tratamiento dependerá del tipo de base de datos</typeparamref>
    /// </summary>
    public interface IEntitySearch<T>
    {

        /// <summary>
        /// Identificador único de cualquier entidad del modelo, generalmente un guid.
        /// </summary>
        string id { get; set; }

        // TODO : el campo de fecha en el entity search aún no es utilizado y existe un timestamp en todos los elementas para investigar si es posible su reemplazo, el objetivo de este campo es uso de TTL (time to live).
        /// <summary>
        /// Fecha de creación del elemento
        /// </summary>
        DateTime created { get; set; }

        /// <summary>
        /// El índice que identifica la entidad, por ejemplo 1 = Persona.
        /// </summary>
        int index { get; set; }


        /// <summary>
        /// diccionario de valores de tipo booleano.
        /// </summary>
        IBoolProperty[] bl { get; set; }
        
        /// <summary>
        /// diccionario de valores de tipo double.
        /// </summary>
        IDblProperty[] dbl { get; set; }


        /// <summary>
        /// diccionario de valor de tipo date
        /// </summary>
        IDtProperty[] dt { get; set; }
        
        /// <summary>
        /// diccionario de valores de tipo enum.
        /// </summary>
        IEnumProperty[] enm { get; set; }

       
        
        /// <summary>
        /// diccionario de valores de tipo número
        /// </summary>
        INum32Property[] num32 { get; set; }

        /// <summary>
        /// diccionario de valores de tipo long
        /// </summary>
        INum64Property[] num64 { get; set; }

        /// <summary>
        /// diccionario de valores de tipo relación
        /// en elemento se podrá relacionar 1 a 1 o 1 a muchos a través de esta propiedad.
        /// </summary>
        IRelatedId[] rel { get; set; }

        /// <summary>
        /// diccionario de valores tipo string.
        /// </summary>
        IStrProperty[] str { get; set; }

        /// <summary>
        /// diccionario de valores de tipo string que serán usados para autocompletar o sugerencias mientras busca.
        /// </summary>
        IStrProperty[] sug { get; set; }


        IProperty<T>[] geo { get; set; }


        /// <summary>
        /// hash de la base de datos que representa el modelo.
        /// </summary>
        string hm { get; set; }


        /// <summary>
        /// hash de la cabecera.
        /// </summary>
        string hh { get; set; }
    }



}