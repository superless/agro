using System.Collections.Generic;
using trifenix.agro.search.model;

namespace trifenix.agro.search.interfaces {
    public interface IAgroSearch {

        void AddEntities(List<EntitySearch> entities);

        EntitiesSearchContainer GetPaginatedEntities(Parameters parameters);

        void DeleteEntities(List<EntitySearch> entities);

    }
}