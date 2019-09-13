using System;

namespace trifenix.agro.model.external
{
    public class ExtPostErrorContainer<T> : ExtPostContainer<T>{

        public Exception InternalException { get; set; }

        public ExtPostContainer<T> GetBase => new ExtPostContainer<T>
        {
            IdRelated = IdRelated,
            Message = Message,
            MessageResult = MessageResult,
            Result = Result
        };


    }
}
