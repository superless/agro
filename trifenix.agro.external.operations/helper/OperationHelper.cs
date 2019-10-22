using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.model.external;
using Cosmonaut.Extensions;
using trifenix.agro.db;
using System.Linq.Expressions;

namespace trifenix.agro.external.operations.helper
{
    public static class OperationHelper
    {


        public static ExtGetContainer<T> GetElement<T>(T element) {
            try
            {
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
            catch (Exception exception)
            {
                return new ExtGetErrorContainer<T>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = exception.Message,
                    InternalException = exception
                };
            }
        }

        public static ExtGetContainer<List<T>> GetElements<T>(List<T> elements)
        {
            try
            {
                return new ExtGetContainer<List<T>>
                {
                    Result = elements,
                    StatusResult = elements.Any() ? ExtGetDataResult.Success : ExtGetDataResult.EmptyResults
                };
            }
            catch (Exception exception)
            {
                return new ExtGetErrorContainer<List<T>>
                {
                    StatusResult = ExtGetDataResult.Error,
                    ErrorMessage = exception.Message,
                    InternalException = exception
                };
            }
        }

        public static async Task<ExtPostContainer<T>> EditElement<T>(string id, T elementToEdit, Func<T, T> transform, Func<T, Task> actionEdit, string noExistsMessage) {
            try
            {
                
                if (elementToEdit == null)
                {
                    return new ExtPostErrorContainer<T>
                    {
                        Message = noExistsMessage,
                        MessageResult = ExtMessageResult.ElementToEditDoesNotExists,
                        IdRelated = id
                    };
                }
                var elementToSave = transform(elementToEdit);


                await actionEdit(elementToSave);

                return new ExtPostContainer<T>
                {
                    Result = elementToSave,
                    IdRelated = id,
                    MessageResult = ExtMessageResult.Ok
                };
            }
            catch (Exception ex)
            {
                return new ExtPostErrorContainer<T>
                {
                    IdRelated = id,
                    MessageResult = ExtMessageResult.Error,
                    Message = ex.Message,
                    InternalException = ex
                };

            }
        }

        public static async Task<ExtPostContainer<string>> CreateElement<T>(IQueryable<T> store,  Func<string, Task<string>> elementToSave, Expression<Func<T, bool>> alreadyExists, string messageAlreadyExists ) where T:DocumentBase
        {

            try
            {
                var element = await store.FirstOrDefaultAsync(alreadyExists);
                if (element != null)
                {
                    return new ExtPostErrorContainer<string>
                    {
                        Message = messageAlreadyExists,
                        MessageResult = ExtMessageResult.ElementToEditDoesNotExists,
                        IdRelated = element.Id
                    };
                }

                var idResult = await elementToSave(Guid.NewGuid().ToString("N"));

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

        public static ExtPostErrorContainer<T> PostNotFoundElementException<T>(string message, string id = null) {
            return new ExtPostErrorContainer<T>
            {
                Message = message,
                InternalException = new Exception(message),
                IdRelated = id,
                MessageResult = ExtMessageResult.ElementToEditDoesNotExists
            };
        }

        public static ExtPostErrorContainer<T> GetException<T>(Exception exc)
        {
            return new ExtPostErrorContainer<T>
            {
                Message = exc.Message,
                InternalException = exc,
                MessageResult = ExtMessageResult.Error
            };
        }
    }
}

