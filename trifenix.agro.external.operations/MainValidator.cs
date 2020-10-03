using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.applicationsReference.agro.Common;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external.Input;
using trifenix.connect.agro_model_input;
using trifenix.connect.mdm_attributes;
using trifenix.connect.util;

namespace trifenix.agro.external.operations
{
    public class MainValidator<T, T_INPUT> : IValidatorAttributes<T_INPUT, T> where T : DocumentBase where T_INPUT : InputBase
    {
        private static List<object> CreateDynamicList(object Obj) => Obj is IList ? (Obj is Array ? ((Array)Obj).Cast<object>().ToList() : new List<object>((IEnumerable<object>)Obj)) : new List<object> { Obj };



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


            var castedCollectionElement = CreateDynamicList(elemento);

            if (!castedCollectionElement.Any())
            {
                return new ResultValidate
                {
                    Valid = true
                };
            }



            // si no es string, no es válido
            if (elemento.GetType() != typeof(string))
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
            var mtd = typeof(CosmosExistElement).GetMethod(nameof(IExistElement.ExistsById), BindingFlags.Public);

            // asigna el elemento generics desde el tipo que mantiene ReferenceAttribute
            var genericMtd = mtd.MakeGenericMethod(attributeLink.entityOfReference);


            // ejecuta el método de busqueda por id, usando el valor de la propiedad.
            var task = (Task<bool>)genericMtd.Invoke(existElement, new[] { elemento.ToString() });

            // ejecuta asíncrono
            await task.ConfigureAwait(false);

            // obtiene la propiedad result
            var resultProperty = task.GetType().GetProperty("Result");

            // obtiene el valor booleano, true si existe.
            var valid = (bool)resultProperty.GetValue(task);



            return new ResultValidate { Valid = valid, Messages = !valid ? new string[] { $"No existe {attributeLink.entityOfReference.Name} con id {elemento}" } : Array.Empty<string>() };

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

            // si no es string, no debería ser validado.
            if (elemento.GetType() != typeof(string))
            {
                return new ResultValidate
                {
                    Valid = false,
                    Messages = new string[] {
                            $"La propiedad del input {propName} debe ser string, el atributo unique es solo para strings"
                        }
                };
            }

            // nombre de la propiedad a buscar
            var propNameDb = string.Empty;

            // valor de la propiedad a buscar
            var value = string.Empty;


            // atributo que vincula un clase input con una clase de base de datos
            var attributeLink = (BaseIndexAttribute)attributes.FirstOrDefault(s => s.GetType() == typeof(BaseIndexAttribute));

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
                            $"La propiedad del input {propName} que tiene que ser validada, no existe en el elemento de la base de datos"
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
                            $"La propiedad del input {propName} que tiene que ser validada, no existe en el elemento de la base de datos (BaseEntitySearch)"
                        }
                };
            }

            // asigna el nombre de la propidad que tiene la propiedad de la clase de la base de datos.
            propNameDb = propNameDb ?? dbPropInfo.Name;


            // realiza la busqueda en la base de datos
            var exists = await existElement.ExistsWithPropertyValue<T>(propNameDb, elemento.ToString(), id);


            // si no existe, significa que esta ok.
            if (!exists)
            {
                return new ResultValidate
                {
                    Valid = true
                };
            }

            return new ResultValidate
            {
                Valid = false,
                Messages = new string[] {
                            $"La propiedad del input {propName} con valor {elemento} existe previamente en la base de datos"
                        }

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
        private Task<ResultValidate> ValidaRequired(object element, string className, string propName, Attribute[] attributes)
        {

            if (element == null || string.IsNullOrWhiteSpace(element.ToString()))
            {
                return Task.FromResult(new ResultValidate
                {
                    Valid = false,
                    Messages = new string[] { $"{className}.{propName} es obligatorio" }
                });
            }
            return Task.FromResult(new ResultValidate
            {
                Valid = true
            });

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
            var nonPrimitiveProps = propsInfo.Where(s => !EnumerationExtension.IsPrimitive(s.PropertyType));

            if (nonPrimitiveProps.Any() && recursive)
            {
                foreach (var propinfo in nonPrimitiveProps)
                {
                    if (propinfo.GetValue(element) != null)
                    {
                        var res = await ValidaProperties<A>(propinfo.GetValue(element), extraValidation, true);
                        if (!res.Valid)
                        {
                            messages.AddRange(res.Messages);
                            valida = false;
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



        public async Task<ResultValidate> Valida(T_INPUT elemento)
        {

            // valida propiedades requeridas
            var requireds = await ValidaProperties<RequiredAttribute>(elemento, ValidaRequired, true);

            // valida propiedades que sus valores deben ser únicos
            var uniques = await ValidaProperties<UniqueAttribute>(
                elemento,
                async (o, className, propName, atributes) =>
                    await ValidaUnique((T_INPUT)o, className, propName, atributes, string.IsNullOrWhiteSpace(elemento.Id) ? null : elemento.Id), false);

            // valida que las referencias existan en la base de datos.
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
