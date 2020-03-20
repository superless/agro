using trifenix.agro.enums;
using trifenix.agro.search.interfaces;

namespace trifenix.agro.search.operations
{
    public class SearchQueries : ISearchQueries
    {
        public string Get(SearchQuery query)
        {
            switch (query)
            {
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