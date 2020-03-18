using trifenix.agro.enums;

namespace trifenix.agro.model.external {
    public class ExtPostContainer<T> {

        public ExtMessageResult MessageResult { get; set; }

        public string Message { get; set; }

        public T Result { get; set; }

        public string IdRelated { get; set; }

    }

}