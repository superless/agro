using trifenix.agro.enums;
using trifenix.agro.enums.query;

namespace trifenix.agro.search.interfaces
{
    public interface ISearchQueries {
        string Get(SearchQuery query);
    }
}