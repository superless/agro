using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace trifenix.connect.util
{





    public static partial class Mdm
    {
public static partial class Reflection {
            /// <summary>
            /// Operaciones reflection para colecciones.
            /// </summary>
            public static class Collections
            {

                /// <summary>
                /// Convierte una lista de objetos a un array tipado (T[]) de manera dinámica,
                /// asignandole el tipo y la lista de objetos a castear.
                /// </summary>
                /// <param name="genericParameterType">Tipo al que se convertirá</param>
                /// <param name="list">listado de objetos a convertir.</param>
                /// <returns>objeto que puede ser casteado a un array tipado (T[])</returns>
                public static object CastToGenericArray(Type genericParameterType, IEnumerable<object> list) => typeof(Collections).GetMethod("CastToArray").MakeGenericMethod(genericParameterType).Invoke(null, new object[] { list });


                /// <summary>
                /// Convierte un listado de objetos a un array tipado.
                /// </summary>
                /// <typeparam name="T">el tipo a convertir</typeparam>
                /// <param name="list">lista de objetos a convertir</param>
                /// <returns>array tipado</returns>
                public static T[] CastToArray<T>(IEnumerable<object> list) => list.Select(element => (T)element).ToArray();

                /// <summary>
                /// Convierte una lista de objetos a una colección con generic (List<T>) de manera dinámica,
                /// asignandole el tipo y la lista de objetos a castear.
                /// </summary>
                /// <param name="genericParameterType">Tipo al que se convertirá</param>
                /// <param name="list">listado de objetos a convertir.</param>
                /// <returns>objeto que puede ser casteado a una lista tipada (List<T>)</returns>
                public static object CastToGenericList(Type genericParameterType, IEnumerable<object> list) => typeof(Collections).GetMethod("CastToList").MakeGenericMethod(genericParameterType).Invoke(null, new object[] { list });

                /// <summary>
                /// Convierte un listado de objetos a una lista tipada (List<T>)
                /// </summary>
                /// <typeparam name="T">tipo</typeparam>
                /// <param name="list">listado de obejtos</param>
                /// <returns>lista tipada</returns>
                public static List<T> CastToList<T>(IEnumerable<object> list) => list.Select(element => (T)element).ToList();

                /// <summary>
                /// Crea una instancia de una clase dinámicamente
                /// </summary>
                /// <typeparam name="T">Tipo de la instancia de objeto a crear</typeparam>
                /// <returns>nueva instancia de un objeto del tipo indicado</returns>
                public static T CreateInstance<T>() => (T)Activator.CreateInstance(typeof(T));

                /// <summary>
                /// Crea una instancia tipada, indicandole el tipo, esta puede se puede convertir en el tipo indicado dinámicamente.
                /// </summary>
                /// <param name="genericParameterType">Tipo de la nueva instancia</param>
                /// <returns>nueva instancia de un objeto del tipo indicado</returns>
                public static object CreateEntityInstance(Type genericParameterType) => typeof(Collections).GetMethod("CreateInstance").MakeGenericMethod(genericParameterType).Invoke(null, null);


                /// <summary>
                /// Retorna un valor o colección tipada, de acuerdo a la metadata de la propiedad
                /// si la propiedad no es una colección, retornará el primer valor de la lista.
                /// si es una colección la casteará a un array o lista del tipo de dato que indica la metadata de la propiedad.
                /// </summary>
                /// <param name="prop">metadata de la propiedad</param>
                /// <param name="values">valor a convertir al tipo que indica la metadata</param>
                /// <returns>valor casteado al tipo que indica la metadata.</returns>
                public static object FormatValues(PropertyInfo prop, List<object> values)
                {
                    if (!IsEnumerableProperty(prop))
                        return ((IEnumerable<object>)values).FirstOrDefault();
                    else
                    {
                        var propType = prop.PropertyType;
                        if (propType.IsArray)
                            return CastToGenericArray(propType.GetElementType(), values);
                        else
                            return CastToGenericList(propType.GetGenericArguments()[0], values);
                    }
                }

            }
        }

        


    }
}
