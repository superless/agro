using System;
using System.Collections.Generic;

namespace trifenix.agro.model.external {
    public class ExtPostErrorContainer<T> : ExtPostContainer<T>{

        public Exception InternalException { get; set; }

        private List<string> _validationMessages;
        public List<string> ValidationMessages {
            get {
                _validationMessages = _validationMessages ?? new List<string>();
                return _validationMessages;
            }
            set { _validationMessages = value; }
        }

        public ExtPostContainer<T> GetBase => new ExtPostContainer<T>
        {
            IdRelated = IdRelated,
            Message = Message,
            MessageResult = MessageResult,
            Result = Result
        };

    }

}