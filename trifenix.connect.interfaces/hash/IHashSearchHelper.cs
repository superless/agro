


using System;

namespace trifenix.connect.interfaces.hash
{

    /// <summary>
    /// Esta interface obliga a asignar un hash para el header, es decir lograr un hash desde el diccionario
    /// que representa el diccionario de un entitySearch, es decir cuales son los nombres de los indices.
    /// Esto permitirá que el que lo use y pueda convertir desde el hash al diccionario podrá conocer los campos.
    /// 
    /// También existe un hash generado por el json de un elemento de base de datos de persistencia,
    /// esto permitirá validar que el entitySearch sea un elemento de la base de datos.
    /// </summary>
    public interface IHashSearchHelper
    {
        /// <summary>
        /// desde el modelo se genera un json que generará un hash
        /// se puede convertir desde un entity a una entidad de base de datos,
        /// validando con el hash si la conversión es correcta
        /// </summary>
        /// <typeparam name="T2">tipo de dato de base de datos de persistencia</typeparam>
        /// <param name="model">elemento a convertir en hash</param>
        /// <returns>Hash del elemento</returns>
        string HashModel(object obj);

        /// <summary>
        /// Obtiene un hash de las cabeceras
        /// toma las enumeraciones existentes, las convierte en un hash
        /// con esto todas las colecciones tendrán un único hash.
        /// Si llegase a modificar los índices de las cabeceras, el hash sería diferente al resto.
        /// </summary>
        /// <typeparam name="T2">Tipo de dato para base de datos persistente</typeparam>
        /// <returns>Hash de cabeceras</returns>
        string HashHeader(Type type);
    }



    
}
