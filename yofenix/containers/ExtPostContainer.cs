using trifenix.connect.mdm.enums;

namespace trifenix.connect.mdm.containers
{
    public class ExtPostContainer<T>
    {
        public ExtMessageResult MessageResult { get; set; }

        public string Message { get; set; }

        public T Result { get; set; }

        public string IdRelated { get; set; }

    }

    



}
