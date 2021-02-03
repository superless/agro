using trifenix.connect.agro.interfaces.search;
using trifenix.connect.agro.model_queries;

namespace trifenix.connect.agro.queries
{
    /// <summary>
    /// Consultas de búsqueda
    /// </summary>
    public class SearchQueries : ISearchQueries {
        public string Get(SearchQuery query) {
            switch (query) {

                /// <summary>
                /// Buscar entidades mediante un id de identidades
                /// </summary>
                case SearchQuery.ENTITIES_WITH_ENTITYID:
                    return SearchQueryRes.ENTITIES_WITH_ENTITYID;

                /// <summary>
                /// Buscar entidades mediante un id de identidades excepto un solo id
                /// </summary>
                case SearchQuery.ENTITIES_WITH_ENTITYID_EXCEPTID:
                    return SearchQueryRes.ENTITIES_WITH_ENTITYID_EXCEPTID;

                /// <summary>
                /// Buscar elemento
                /// </summary>
                case SearchQuery.GET_ELEMENT:
                    return SearchQueryRes.GET_ELEMENT;
                default:
                    return string.Empty;
            }
        }
    }

}