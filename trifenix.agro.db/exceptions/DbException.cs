using System;

namespace trifenix.agro.db.exceptions
{
    public class DbException<T> : BaseException<T> where T : DocumentBase
    {
        public DbException(T docBase, Exception internalException) : base(docBase)
        {   
            InternalException = internalException;
            
        }

        public Exception InternalException { get; }

        public override string Message => $"el elemento de tipo {DbObject.GetType()} tuvo un error de tipo {InternalException.Message}";

    }
}
