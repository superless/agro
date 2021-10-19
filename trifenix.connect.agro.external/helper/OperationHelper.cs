using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.agro.external.operations.helper
{

    /// <summary>
    /// Operaciones comunes para resultados de base de datos con el fin de obtener un contenedor como respuesta
    /// qur almacene el mismo elemento.
    /// </summary>
    public static class OperationHelper
    {

        /// <summary>
        /// Crea un contenedor con el elemento que recibe como parametro, si es nulo regresa emptyresults
        /// </summary>
        /// <typeparam name="T">Tipo de elemento a contener</typeparam>
        /// <param name="element">elemento a contener</param>
        /// <returns>Contenedor con elemento</returns>
        public static ExtGetContainer<T> GetElement<T>(T element) {
            if (element == null)
            {
                return new ExtGetContainer<T>
                {
                    Result = element,
                    StatusResult = ExtGetDataResult.EmptyResults
                };
            }

            return new ExtGetContainer<T>
            {
                Result = element,
                StatusResult = ExtGetDataResult.Success
            };
        }


        /// <summary>
        /// Retorna un contenedor con una lista de elementos
        /// </summary>
        /// <typeparam name="T">Tipo de la lista</typeparam>
        /// <param name="elements">lista de elementos</param>
        /// <returns>Contenedor con lista de elementos</returns>
        public static ExtGetContainer<List<T>> GetElements<T>(List<T> elements)
        {
            if (elements == null)
            {
                return new ExtGetContainer<List<T>>
                {
                    Result = null,
                    StatusResult = ExtGetDataResult.EmptyResults
                };
            }

            return new ExtGetContainer<List<T>>
            {
                Result = elements,
                StatusResult = elements.Any() ? ExtGetDataResult.Success : ExtGetDataResult.EmptyResults
            };
        }

        
        

        

        

        /// <summary>
        /// Lanza excepción si el elemento no existe
        /// </summary>
        /// <typeparam name="T">Tipo de Elemento</typeparam>
        /// <param name="message">mensaje de error</param>
        /// <param name="id">id del elemento origen del error (opcional)</param>
        /// <returns>post contenedor</returns>
        public static ExtPostErrorContainer<T> PostNotFoundElementException<T>(string message, string id = null) {
            return new ExtPostErrorContainer<T>
            {
                Message = message,
                InternalException = new Exception(message),
                IdRelated = id,
                MessageResult = ExtMessageResult.ElementToEditDoesNotExists
            };
        }


        /// <summary>
        /// Lanza excepción de tipo Get
        /// </summary>
        /// <typeparam name="T">Tipo de elemento</typeparam>
        /// <param name="exc">excepción a incluír</param>
        /// <returns>contenedor con la excepción</returns>
        public static ExtPostErrorContainer<T> GetPostException<T>(Exception exc)
        {
            return new ExtPostErrorContainer<T>
            {
                Message = exc.Message,
                InternalException = exc,
                MessageResult = ExtMessageResult.Error
            };
        }

        public static ExtGetErrorContainer<T> GetException<T>(Exception exc)
        {
            return new ExtGetErrorContainer<T>
            {
                
                InternalException = exc,
                ErrorMessage = exc.Message,
                StatusResult = ExtGetDataResult.Error
            };
        }
    }
}

