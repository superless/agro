using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.model.external;
using trifenix.agro.db;
using System.Linq.Expressions;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.enums;
using trifenix.agro.db.interfaces.agro.common;

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
        /// Metodo estático que permite editar un elemento y enviar los mensajes que correspondan de acuerdo al resultado de la operación.
        /// </summary>
        /// <typeparam name="T">Tipo de entidad a editar</typeparam>
        /// <param name="idElementToEdit">identificador del elemento a editar</param>
        /// <param name="elementToEdit">elemento a editar, sin cambios</param>
        /// <param name="transform">Operación que permitirá cambiar los campos</param>
        /// <param name="actionEdit">acción que editará el elemento en la base de datos</param>
        /// <param name="noExistsMessage">mensaje personalizado en caso de no existir el elemento</param>
        /// <returns>Contenedor con el elemento a editar</returns>
        public static async Task<ExtPostContainer<T>> EditElement<T>(ICommonDbOperations<T> dbOper, IQueryable<T> store, string idElementToEdit, T elementToEdit, Func<T, T> transform, Func<T, Task> actionEdit, string noExistsMessage, Expression<Func<T, bool>> alreadyExists, string messageAlreadyExists) where T : DocumentBase
         {
            try
            {
                string idAlreadyExist = await AlreadyExist(dbOper, store, alreadyExists);
                if (idAlreadyExist != null)
                {
                    return new ExtPostErrorContainer<T>
                    {
                        Message = messageAlreadyExists,
                        MessageResult = ExtMessageResult.ElementAlreadyExists,
                        IdRelated = idAlreadyExist
                    };
                }

                if (elementToEdit == null)
                {
                    return new ExtPostErrorContainer<T>
                    {
                        Message = noExistsMessage,
                        MessageResult = ExtMessageResult.ElementToEditDoesNotExists,
                        IdRelated = idElementToEdit
                    };
                }
                var elementToSave = transform(elementToEdit);

                await actionEdit(elementToSave);

                return new ExtPostContainer<T>
                {
                    Result = elementToSave,
                    IdRelated = idElementToEdit,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception ex)
            {
                return new ExtPostErrorContainer<T>
                {
                    IdRelated = idElementToEdit,
                    MessageResult = ExtMessageResult.Error,
                    Message = ex.Message,
                    InternalException = ex
                };

            }
        }



        
        

        //TODO: Deprecar
        /// <summary>
        /// Crea elemento en la base de datos y crea un contenedor con la respuesta de acuerdo al resultado de la operación.
        /// </summary>
        /// <typeparam name="T">Tipo del elemento a crear</typeparam>
        /// <param name="store">Store de elementos a consultar</param>
        /// <param name="elementToSave">función que guarda el elemento en la base de datos</param>
        /// <param name="alreadyExists">función que comprueba si elemento ya existe</param>
        /// <param name="messageAlreadyExists">mensaje si el elemento ya existe</param>
        /// <returns>Contenedor con el id del elemento creado o el error del resultado</returns>
        public static async Task<ExtPostContainer<string>> CreateElement<T>(ICommonDbOperations<T> dbOper, IQueryable<T> store,  Func<string, Task<string>> elementToSave, Expression<Func<T, bool>> alreadyExists, string messageAlreadyExists) where T:DocumentBase
        {
            try
            {
                string idAlreadyExist = await AlreadyExist(dbOper, store, alreadyExists);
                if (idAlreadyExist != null)
                {
                    return new ExtPostErrorContainer<string>
                    {
                        Message = messageAlreadyExists,
                        MessageResult = ExtMessageResult.ElementAlreadyExists,
                        IdRelated = idAlreadyExist
                    };
                }

                string idResult = await elementToSave(Guid.NewGuid().ToString("N"));

                return new ExtPostContainer<string>
                {
                    IdRelated = idResult,
                    Result = idResult,
                    MessageResult = ExtMessageResult.Ok
                };

            }
            catch (Exception ex)
            {
                return new ExtPostErrorContainer<string>
                {
                    InternalException = ex,
                    Message = ex.Message,
                    MessageResult = ExtMessageResult.Error
                };
            }
        }

        public static async Task<string> AlreadyExist<T>(ICommonDbOperations<T> dbOper, IQueryable<T> store, Expression<Func<T, bool>> alreadyExists) where T : DocumentBase
        {
            var element = await dbOper.FirstOrDefaultAsync(store, alreadyExists);
            return element?.Id;
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

