using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm_attributes;
using trifenix.connect.search_mdl;

namespace trifenix.connect.util
{




    /// <summary>
    /// Todos los métodos relacionados con la obtención de metadata y valores desde el modelo de clases y la conversión de esta 
    /// al modelo de metada de trifenix y viceversa.
    /// </summary>
    public static partial class Mdm
    {

        /// <summary>
        /// Retorna un objeto desde un entitySearch, el tipo del objeto de retorno será del tipo que utilice el atributo EntityIndexAttribute .
        /// para esto buscará todas las clases que tnengan el atributo EntityIndexAttribute que vincula la clase con el índice
        /// del entitySearch, una vez encontrada hará lo mismo con los atributos de cada propiedad para finalmente crear un objeto tipado con todos los valores del entitySearch.
        /// </summary>
        /// <typeparam name="T">Las entidades tienen un tipo de dato geo, que depende de la base de datos a usar.</typeparam>        
        /// <param name="entitySearch">entitySearch a convertir</param>
        /// <param name="anyElementInAssembly">assembly donde buscar la clase que sea del tipo de la entidad</param>
        /// <param name="nms">namespace donde se encuentra la clase que sea del tipo de entidad</param>
        /// <returns>objeto de una clase que representa una entidad</returns>
        public static object GetEntityFromSearch<T>(IEntitySearch<T> entitySearch, Type anyElementInAssembly, string nms)
        {
            
            // obtiene el tipo de clase de acuerdo al índice de la entidad.
            var type = Reflection.GetEntityType(entitySearch.index, anyElementInAssembly, nms);


            // crea una nueva instancia del tipo determinado por la entidad
            // por ejemplo, si el indice de entidad correspondiera a 1 que es Persona, esta sería la clase persona.
            var entity = Reflection.Collections.CreateEntityInstance(type);


            // asigna el id del objeto convertido
            // todas los elementos de la base de datos tienn la propiedad id.
            type.GetProperty("Id")?.SetValue(entity, entitySearch.id);


            // busca todas las propiedades que tengan el atributo baseIndexAttribute que identifica la metadata donde reside el índice y el tipo de dato.
            var props = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(BaseIndexAttribute), true)).ToList();


           
            // recorre las propiedades de una clase y le asigna los valores correspondientes a las propiedades del entitySearch
            props.ForEach(prop => {

                // obtiene el atributo y su metadata
                var attr = prop.GetCustomAttribute<BaseIndexAttribute>(true);

                // con la metadata de la propiedad (índice, tipo de dato y si es o no)
                var values = Reflection.Collections.FormatValues(prop, GetValues<T>(entitySearch, attr.IsEntity, attr.KindIndex, attr.Index));

                prop.SetValue(entity, values);
            });
            return entity;
        }


        /// <summary>
        /// Obtiene 
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static IRelatedId GetLocalRelatedId(KeyValuePair<BaseIndexAttribute, object> attribute, Type typeToCast)
        {

            if (!CheckImplementsIRelatedId(typeToCast))
            {
                throw new Exception("Debe implementar IRelatedId");
            }
            if (attribute.Key.IsEntity && attribute.Key.KindIndex == (int)KindEntityProperty.LOCAL_REFERENCE)
            {
                return GetEntityProperty(attribute.Key.Index, (string)attribute.Value, typeToCast);
            }
            return null;
        }

        public static IRelatedId GetRelatedId(KeyValuePair<BaseIndexAttribute, object> attribute, Type typeToCast)
        {
            if (attribute.Key.IsEntity && attribute.Key.KindIndex == (int)KindEntityProperty.REFERENCE)
            {
                return GetEntityProperty(attribute.Key.Index, (string)attribute.Value, typeToCast);
            }
            return null;
        }


        /// <summary>
        /// Retorna el valor o lista de valores de una propiedad, desde una entidad.
        /// en caso de una referencia local
        /// </summary>
        /// <typeparam name="T">Tipo de valor a entregar</typeparam>
        /// <param name="entitySearch">Entidad a convertir</param>
        /// <param name="isEntity">Determina si la propiedad que se desea obtener es de tipo entidad o es una propiedad primitiva (DateTime, número, etc.)</param>
        /// <param name="typeRelated"></param>
        /// <param name="indexProperty"></param>
        /// <param name="anyElementInAssembly"></param>
        /// <param name="nms"></param>
        /// <param name="sEntity"></param>
        /// <returns></returns>
        public static List<object> GetValues<T>(IEntitySearch<T> entitySearch, bool isEntity, int typeRelated, int indexProperty, ISearchEntity<T> sEntity = null,Type anyElementInAssembly = null, string nms = null)
        {

            if ((sEntity == null || anyElementInAssembly == null || string.IsNullOrWhiteSpace(nms)) && isEntity)
            {
                throw new Exception("si el tipo a recuperar es de tipo entidad ");
            }
            List<object> values = new List<object>();

            if (isEntity)
            {
                var relatedEntity = (KindEntityProperty)typeRelated;
                switch (relatedEntity)
                {
                    case KindEntityProperty.REFERENCE:
                        return (List<object>)entitySearch.rel?.ToList().FindAll(relatedId => relatedId.index == indexProperty).Select(s => s.id) ?? values;
                    case KindEntityProperty.LOCAL_REFERENCE:
                        return entitySearch.rel?.ToList().FindAll(relatedId => relatedId.index == indexProperty).Select(relatedId => GetEntityFromSearch(sEntity.GetEntity(indexProperty, relatedId.id), anyElementInAssembly, nms)).ToList() ?? values;
                    default:
                        return null;
                }

            }
            var props = (KindProperty)typeRelated;

            switch (props)
            {
                case KindProperty.STR:
                    return GetPropValues(entitySearch.str, indexProperty).Cast<object>().ToList();

                case KindProperty.SUGGESTION:
                    return GetPropValues(entitySearch.sug, indexProperty).Cast<object>().ToList();
                case KindProperty.NUM64:
                    return GetPropValues(entitySearch.num64, indexProperty).Cast<object>().ToList();
                case KindProperty.NUM32:
                    return GetPropValues(entitySearch.num32, indexProperty).Cast<object>().ToList();
                case KindProperty.DBL:
                    return GetPropValues(entitySearch.dbl, indexProperty).Cast<object>().ToList();
                case KindProperty.BOOL:
                    return GetPropValues(entitySearch.bl, indexProperty).Cast<object>().ToList();
                case KindProperty.GEO:
                    return GetPropValues(entitySearch.geo, indexProperty).Cast<object>().ToList();
                case KindProperty.ENUM:
                    return GetPropValues(entitySearch.enm, indexProperty).Cast<object>().ToList();
                case KindProperty.DATE:
                    return GetPropValues(entitySearch.dt, indexProperty).Cast<object>().ToList();
                default:
                    return null;
            }
        }


        /// <summary>
        /// retorna los valores desde una colección de propiedades.
        /// </summary>
        /// <typeparam name="T">Tipo de valor de la propiedad</typeparam>
        /// <param name="props">propiedades que serán usadas para retornar los valores</param>
        /// <param name="index">índice de la propiedad</param>
        /// <returns></returns>
        public static T[] GetPropValues<T>(IProperty<T>[] props, int index) => props.Where(s => s.index == index).Select(s => s.value).ToArray();




        /// <summary>
        /// Verifica si un tipo es una propiedad (IProperty<>)
        /// </summary>
        /// <see cref="IProperty{T}"/>
        /// <param name="typeToCheck">tipo a verificar</param>
        /// <returns>true, si implementa IProperty</returns>
        public static bool CheckImplementsIProperty(Type typeToCheck) {
            return typeToCheck.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IProperty<>));
        }

        /// <summary>
        /// Verifica si un tipo es una propiedad de tipo entidad
        /// </summary>
        /// <see cref="IRelatedId"/>
        /// <param name="typeToCheck">tipo a verificar</param>
        /// <returns>true, si implementa IProperty</returns>
        public static bool CheckImplementsIRelatedId(Type typeToCheck)
        {
            return typeToCheck.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRelatedId<>));
        }


        /// <summary>
        /// Una propiedad de un entitySearch es un contenedor con un índice que mapea una propiedad de una clase y el valor que tiene esa propiedad ,
        /// de acuerdo al tipo de propiedad de la clase será el tipo de contenedor que retornará.        
        /// </summary>
        /// <typeparam name="T">Tipo de propiedad (string, double, enum, etc.)</typeparam>        
        /// <see cref="IEntitySearch{T}" />
        /// <see cref="IProperty{T}"/>
        /// <param name="index">índice para la propiedad</param>
        /// <param name="value">valor que se asignará a la propiedad</param>
        /// <param name="typeToCast">Tipo de la nueva propiedad a retornar</param>
        /// <returns>propiedad de un entitySearch con su índice y valor</returns>
        public static IProperty<T> GetProperty<T>(int index, object value, Type typeToCast)
        {
            if (!CheckImplementsIProperty(typeToCast))
            {
                throw new Exception("El tipo debe implementar IProperty");
            }

            // crea una nueva instancia de una propiedad (entrada de un entity search).
            var element = (IProperty<T>)Reflection.Collections.CreateEntityInstance(typeToCast);

            // asigna el índice
            element.index = index;

            // Castea el valor al tipo que se indica.
            try
            {
                element.value = (T)value;
            }
            catch (Exception e)
            {
                if (e.Message.Equals("Unable to cast object of type 'System.Int32' to type 'System.Int64'."))
                    element.value = (T)(object)Convert.ToInt64(value);
                else
                    throw;
            }
            return element;
        }


        /// <summary>
        /// Un EntitySearch se compone de propiedades que relacionan otros EntitySearch
        /// estas propiedades tienen el índice que identifica el tipo de entidad (Persona, Producto o cualquier tipo de agrupación) y el id que identifica un elemento dentro de una base de datos.
        /// este método crea una propiedad de este tipo
        /// </summary>
        /// <param name="index">índice del tipo de entidad</param>
        /// <param name="id">identificador de la entidad</param>
        /// <param name="typeToCast">Tipo al que debe ser convertido (debe implementar IRelatedId)</param>
        /// <returns></returns>
        public static IRelatedId GetEntityProperty(int index, string id, Type typeToCast) {

            if (!CheckImplementsIRelatedId(typeToCast))
            {
                throw new Exception("El tipo debe implementar IRelatedId");
            }

            // crea una nueva instancia de una propiedad (entrada de un entity search).
            var element = (IRelatedId)Reflection.Collections.CreateEntityInstance(typeToCast);

            // asigna el índice
            element.index = index;
            element.id = id;
            return element;
        }
    }
    }
}
