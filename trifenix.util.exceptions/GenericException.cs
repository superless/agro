using System;

namespace trifenix.util.exceptions
{
    [Serializable]
    public class GenericException : Exception
    {
        public GenericException() { }
        public GenericException(string message) : base(message) { }
        public GenericException(string message, System.Exception inner) : base(message, inner) { }

        // Constructor needed for serialization 
        // when exception propagates from a remoting server to the client.
        protected GenericException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
