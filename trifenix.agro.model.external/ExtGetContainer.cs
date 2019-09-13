using System;

namespace trifenix.agro.model.external
{
    public class ExtGetContainer<T>
    {
        public ExtGetDataResult StatusResult { get; set; }

        public T Result { get; set; }

        public string ErrorMessage { get; set; }

    }

    public class ExtGetErrorContainer<T> : ExtGetContainer<T>
    {
        public Exception InternalException { get; set; }

        public ExtGetContainer<T> GetBase => new ExtGetContainer<T> {
            ErrorMessage = ErrorMessage,
            Result = Result,
            StatusResult = StatusResult

        };


    }



}
