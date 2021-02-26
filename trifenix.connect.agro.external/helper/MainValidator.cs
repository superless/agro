using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using trifenix.connect.entities.cosmos;
using trifenix.connect.input;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.validation_attributes;
using trifenix.connect.mdm_attributes;
using trifenix.connect.util;

namespace trifenix.connect.agro.external.helper
{

    /// <summary>
    /// Clase principal de validación de input en base a atributos
    /// donde según los atributos de validación que tengan las propiedades de la clase input
    /// se validará acordemente.
    /// </summary>
    /// <typeparam name="T">Elemento de la base de datos al que correspoende</typeparam>
    /// <typeparam name="T_INPUT"></typeparam>
    public class MainValidator<T, T_INPUT> : IValidatorAttributes<T_INPUT> where T : DocumentBase where T_INPUT : InputBase
    {
        private readonly IExistElement existElement;

        public MainValidator(IExistElement existElement)
        {
            this.existElement = existElement;
        }

        public IExistElement GetExistElement()
        {
            return existElement;
        }

        /// <summary>
        /// Valida las propiedades que usen el atributo Reference, al detectar este atributo, validará si el elemento existe en la base de datos.
        /// </summary>
        /// <param name="elemento">elemento a validar</param>
        /// <param name="className">nombre de la clase a la que pertenece el elemento</param>
        /// <param name="propName">nombre de la propiedad</param>
        /// <param name="attributes">atributos de la propiedad</param>
        /// <returns>resultado que integra si es valido y los mensajes de error</returns>
        private async Task<ResultValidate> ValidaReference(object elemento, string className, string propName, Attribute[] attributes)
        {
            // si es nulo es válido, no válida nulos
            if (elemento == null)
            {
                return new ResultValidate
                {
                    Valid = true
                };
            }

            // se crea colección desde el objeto
            var castedCollectionElement = Mdm.CreateDynamicList(elemento);

            // si es vacia, no valida, requerido valida.
            if (!castedCollectionElement.Any())
            {
                return new ResultValidate
                {
                    Valid = true
                };
            }

            // si no es string, no es válido
            if (elemento.GetType() != typeof(string) && elemento.GetType() != typeof(List<string>) && elemento.GetType() != typeof(string[]))
            {
                return new ResultValidate
                {
                    Valid = false,
                    Messages = new string[] {
                            $"La propiedad del input {className}.{propName} debe ser string, el atributo reference es solo para strings"
                        }
                };
            }

            //obtiene el atributo desde el listado de atributos.
            var attributeLink = (ReferenceAttribute)attributes.FirstOrDefault(s => s.GetType() == typeof(ReferenceAttribute));

            // método que permite saber si un elemento existe
            var mtd = typeof(IExistElement).GetMethod(nameof(IExistElement.ExistsById));

            // asigna el elemento generics desde el tipo que mantiene ReferenceAttribute
            var genericMtd = mtd.MakeGenericMethod(attributeLink.entityOfReference);



            bool valid = true;

            var lst = new List<string>();

            foreach (var item in castedCollectionElement)
            {

                var strElement = (string)item;

                // si es vacio no se valida
                if (string.IsNullOrWhiteSpace(strElement)) break;
                // ejecuta el método de busqueda por id, usando el valor de la propiedad.
                var task = (Task<bool>)genericMtd.Invoke(existElement, new[] { item.ToString() });

                // ejecuta asíncrono
                await task.ConfigureAwait(false);

                // obtiene la propiedad result
                var resultProperty = task.GetType().GetProperty("Result");

                // obtiene el valor booleano, true si existe.
                var localValid = (bool)resultProperty.GetValue(task);

                if (!localValid)
                {
                    valid = localValid;
                    lst.Add($"No existe {attributeLink.entityOfReference.Name} con id {item}");
                }
            }

            



            return new ResultValidate { Valid = valid, Messages = !valid ? lst.ToArray() : Array.Empty<string>() };

        }

        /// <summary>
        /// Valida las propiedades que tengan el atributo Unique,
        /// el atributo indica una propiedad que debe existir un solo valor en toda la base de datos
        /// por tanto, buscará si ya existe el mismo valor en la misma propiedad para una colección de objetos.
        /// La colección de objetos esta determinado por el tipo de objeto del elemento.
        /// si se asigna un id, no considerará el elemento con ese id en la busqueda.
        /// </summary>
        /// <param name="elemento">elemento a validar, una propiedad de un objeto</param>
        /// <param name="className">nombre de la clase</param>
        /// <param name="propName">nombre de la propiedad del elemento</param>
        /// <param name="attributes">atributos encontrados</param>
        /// <param name="id">si se asigna un id, significa que buscará la propiedad y el valor, pero no para ese id.</param>
        /// <returns>resultado que integra si es valido y los mensajes de error</returns>
        private async Task<ResultValidate> ValidaUnique(object elemento, string className, string propName, Attribute[] attributes, string id = null)

        {
            // si el elemento es nulo, es válido.
            if (elemento == null)
            {
                return new ResultValidate
                {
                    Valid = true
                };
            }

            // se crea colección desde el objeto, con el fin de validar colecciones
            var castedCollectionElement = Mdm.CreateDynamicList(elemento);

            // si la colección esta vacia no validará.
            if (!castedCollectionElement.Any())
            {
                return new ResultValidate
                {
                    Valid = true
                };
            }


            // si no es string, no debería ser validado.
            if (elemento.GetType() != typeof(string) && elemento.GetType()!= typeof(List<string>) && elemento.GetType() != typeof(string[]))
            {

                return new ResultValidate
                {
                    Valid = false,
                    Messages = new string[] {
                            $"La propiedad del input {className}.{propName} debe ser string, el atributo unique es solo para strings"
                        }
                };
            }

            // nombre de la propiedad a buscar
            var propNameDb = string.Empty;

            // valor de la propiedad a buscar
            var value = string.Empty;


            // atributo que vincula un clase input con una clase de base de datos
            var attributeLink = (BaseIndexAttribute)attributes.FirstOrDefault(s => s.GetType().IsSubclassOf(typeof(BaseIndexAttribute)));

            // si es nulo, se buscará una propiedad con el mismo nombre en la clase de la base de datos (T).
            if (attributeLink == null)
            {
                // información de las propiedad de la clase de la base de datos, que tenga el mismo bombre de la clase input.
                var propDbInfo = typeof(T).GetProperty(propName);

                // si es nulo, significa que no existe vinculo entre la clase input y la clase de la base de datos.
                if (propDbInfo == null)
                {
                    return new ResultValidate
                    {
                        Valid = false,
                        Messages = new string[] {
                            $"La propiedad del input {className}.{propName} que tiene que ser validada, no existe en el elemento de la base de datos"
                        }
                    };
                }
                // asigna un nombre para lo propiedad
                propNameDb = propDbInfo.Name;
            }

            // busca todas las propiedades que tengan un atributo fenix, que vincula input con base de datos.
            var baseIndexAttributes = typeof(T).GetProperties().Where(pinfo => Mdm.Reflection.Attributes.GetAttribute<BaseIndexAttribute>(pinfo) != null);


            // busca una propiedad de la clase de la base de datos, que tenga los mismos índices.
            var dbPropInfo = baseIndexAttributes.FirstOrDefault(s =>
            {
                var attribute = Mdm.Reflection.Attributes.GetAttribute<BaseIndexAttribute>(s);

                return attribute.IsEntity == attributeLink.IsEntity && attribute.Index == attributeLink.Index && attribute.KindIndex == attributeLink.KindIndex;
            });

            // si no existe un vinculo, significa que no existe un vinculo entre la propiedad del input y el de la base de datos.
            if (dbPropInfo == null)
            {
                return new ResultValidate
                {
                    Valid = false,
                    Messages = new string[] {
                            $"La propiedad del input {className}.{propName} que tiene que ser validada, no existe en el elemento de la base de datos (BaseEntitySearch)"
                        }
                };
            }

            // asigna el nombre de la propidad que tiene la propiedad de la clase de la base de datos.
            propNameDb = !string.IsNullOrWhiteSpace(propNameDb)? propNameDb : dbPropInfo.Name;


            bool valid = true;

            var lst = new List<string>();

            foreach (var item in castedCollectionElement)
            {
                // realiza la busqueda en la base de datos
                var localExists = await existElement.ExistsWithPropertyValue<T>(propNameDb, elemento.ToString(), id);

                if (localExists)
                {
                    valid = !localExists;
                    lst.Add($"La propiedad del input {className}.{propName} con valor {elemento} existe previamente en la base de datos");

                    
                }

            }

            return new ResultValidate
            {
                Valid = valid,
                Messages = lst.ToArray()

            };






        }


        /// <summary>
        /// Valida si un elemento es requerido, si es el caso verificará si existe un valor en esa propíedad
        /// </summary>
        /// <param name="element">objeto a validar</param>
        /// <param name="className">Nombre de la clase a la que pertenece el objeto</param>
        /// <param name="propName">la propiedad que pertenece el objeto</param>
        /// <param name="attributes">atributos encontrados en la propiedad</param>
        /// <returns>resultado que integra si es valido y los mensajes de error</returns>
        private async Task<ResultValidate> ValidaRequired(object element, string className, string propName, Attribute[] attributes)
        {
            // si es nulo no es válido
            if (element == null)
            {
                return new ResultValidate
                {
                    Valid = false,
                    Messages = new string[] {
                            $"La propiedad del input {className}.{propName} es obligatorio" }
                };
            }

            // se crea colección desde el objeto
            var castedCollectionElement = Mdm.CreateDynamicList(element);

            // si la lista es vacia, es inválido.
            if (!castedCollectionElement.Any())
            {
                return new ResultValidate
                {
                    Valid = false,
                    Messages = new string[] {
                            $"La propiedad del input {className}.{propName} es obligatorio, tampoco puede ser una lista vacia" }
                };
            }

            

            
           

            //obtiene el atributo desde el listado de atributos que sean requerido.
            var attributeLink = (RequiredAttribute)attributes.FirstOrDefault(s => s.GetType() == typeof(RequiredAttribute));

            var valid = true;
            var lst = new List<string>();

            foreach (var item in castedCollectionElement)
            {

                // obtiene el tipo, según el tipo validará.
                var type = item.GetType();
                if (type == typeof(int) || type == typeof(int?) || type == typeof(double) || type == typeof(double?) || type == typeof(decimal) || type == typeof(decimal?))
                {
                    if (item==null)
                    {
                        lst.Add($"{className}.{propName} es obligatorio");
                        valid = false;
                    } else
                    {
                        if (int.TryParse(item.ToString(), out var intResult))
                        {
                            if (intResult == 0)
                            {
                                lst.Add($"{className}.{propName} es obligatorio, ni puede cer cero");
                                valid = false;
                            }
                        }
                        
                    }
                }

                if (type == typeof(bool?) || type == typeof(DateTime?))
                {

                    if (item == null)
                    {
                        lst.Add($"{className}.{propName} es obligatorio");
                        valid = false;
                    }
                    
                }

                


                if (item == null || string.IsNullOrWhiteSpace(item.ToString()))
                {
                    lst.Add($"{className}.{propName} es obligatorio");
                    valid = false;
                }
            }

            return new ResultValidate { Valid = valid, Messages = lst.ToArray() };

        }


        /// <summary>
        /// Valida recursivamente una instancia de un objeto, de acuerdo al método para validar,
        /// si es recursivo, busca propiedades de tipo clase y realiza la misma validación.
        /// </summary>
        /// <typeparam name="A">Tipo de atributo a buscar</typeparam>
        /// <param name="element">instancia de objeto donde se buscaran las propiedades que tengan el atributo</param>
        /// <param name="extraValidation">Función que ejecutará la validación</param>
        /// <param name="recursive">determina si la busqueda de atributos es recursiva, si fuese así buscará todas las clases que tiene un objeto y buscará dentro de este el atributo en cada propiedad</param>
        /// <returns>resultado que integra si es valido y los mensajes de error</returns>
        private async Task<ResultValidate> ValidaProperties<A>(object element, Func<object, string, string, Attribute[], Task<ResultValidate>> extraValidation, bool recursive) where A : Attribute
        {

            var propsInfo = element.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(A), true));
            var messages = new List<string>();
            bool valida = true;


            foreach (var item in propsInfo)
            {
                var validation = await extraValidation(item.GetValue(element), element.GetType().Name, item.Name, item.GetCustomAttributes<Attribute>().ToArray());

                if (!validation.Valid)
                {
                    valida = false;
                    messages.AddRange(validation.Messages);
                }
            }

            
            // propiedades no primitivas
            var nonPrimitiveProps = element.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public ).Where(s => !EnumerationExtension.IsPrimitiveAndCollection(s.PropertyType) && !s.PropertyType.IsEnum);

            if (nonPrimitiveProps.Any() && recursive)
            {
                foreach (var propinfo in nonPrimitiveProps)
                {
                    if (propinfo.GetValue(element) != null)
                    {
                        var castedCollectionElement = Mdm.CreateDynamicList(propinfo.GetValue(element));
                        foreach (var item in castedCollectionElement)
                        {
                            var res = await ValidaProperties<A>(item, extraValidation, true);
                            if (!res.Valid)
                            {
                                messages.AddRange(res.Messages);
                                valida = false;
                            }
                        }

                        
                    }

                }


            }



            return new ResultValidate
            {
                Valid = valida,
                Messages = messages.ToArray()
            };

        }




        /// <summary>
        /// Valida un input de usuario
        /// </summary>
        /// <param name="elemento">input de usuario</param>
        /// <returns>elemento contenedor con el resultado y los mensajes de error</returns>
        public async Task<ResultValidate> Valida(T_INPUT elemento)
        {

            // si tiene un id, se debe comprobar que existe
            if (!string.IsNullOrWhiteSpace(elemento.Id))
            {
                var result = await ValidaReference(elemento.Id, typeof(T_INPUT).Name, "id", new Attribute[] { new ReferenceAttribute(typeof(T)) });
                if (!result.Valid)
                {
                    result.Valid = false;
                    result.Messages = new string[] { $"El {typeof(T_INPUT).Name} con id: {elemento.Id} no existe en la base de datos" };
                    return result;
                }
            }
            // valida propiedades requeridas, buscando de igual manera dentro de los elementos.
            var requireds = await ValidaProperties<RequiredAttribute>(elemento, ValidaRequired, true);

            // valida propiedades que sus valores deben ser únicos, no aplica a las clases internas.
            var uniques = await ValidaProperties<UniqueAttribute>(
                elemento,
                async (o, className, propName, atributes) =>
                    await ValidaUnique(o, className, propName, atributes, string.IsNullOrWhiteSpace(elemento.Id) ? null : elemento.Id), false);

            // valida que las referencias existan en la base de datos. verifica clases internas
            var references = await ValidaProperties<ReferenceAttribute>(elemento, ValidaReference, true);


            var messages = new List<string>();
            var valid = true;

            if (!requireds.Valid)
            {
                valid = false;
                messages.AddRange(requireds.Messages);
            }


            if (!uniques.Valid)
            {
                valid = false;
                messages.AddRange(uniques.Messages);
            }
            if (!references.Valid)
            {
                valid = false;
                messages.AddRange(references.Messages);
            }


            return new ResultValidate
            {
                Valid = valid,
                Messages = messages.ToArray()
            };
        }

       
    }
}
