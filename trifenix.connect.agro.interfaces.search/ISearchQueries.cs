using trifenix.connect.agro.model_queries;

namespace trifenix.connect.agro.interfaces.search
{
    public interface ISearchQueries {
        string Get(SearchQuery query);
    }
}