using trifenix.connect.agro.interfaces.search;
using trifenix.connect.agro.model_queries;

namespace trifenix.connect.agro.queries
{

    public class SearchQueries : ISearchQueries {
        public string Get(SearchQuery query) {
            switch (query) {
                case SearchQuery.ENTITIES_WITH_ENTITYID:
                    return SearchQueryRes.ENTITIES_WITH_ENTITYID;
                case SearchQuery.ENTITIES_WITH_ENTITYID_EXCEPTID:
                    return SearchQueryRes.ENTITIES_WITH_ENTITYID_EXCEPTID;
                case SearchQuery.GET_ELEMENT:
                    return SearchQueryRes.GET_ELEMENT;
                default:
                    return string.Empty;
            }
        }
    }

}