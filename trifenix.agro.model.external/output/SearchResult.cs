using System;

namespace trifenix.agro.model.external.output {
    public class SearchResult<T> {
        public long Total { get; set; }
        public T[] Elements { get; set; }
    }

    public class Element {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }
    }
}
