namespace trifenix.agro.model.external.output {
    public class SearchResult<T> {
        public long Total { get; set; }
        public T[] Elements { get; set; }
    }
}
