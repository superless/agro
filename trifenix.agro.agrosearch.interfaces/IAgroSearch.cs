using System.Collections.Generic;
using trifenix.agro.search.model;

namespace trifenix.agro.search.interfaces {
    public interface IAgroSearch {

        void AddEntities(List<EntitySearch> entities);


        void AddSimpleEntities(List<SimpleSearch> simpleEntities);


        void AddComments(List<CommentSearch> comments);

        EntitiesSearchContainer GetPaginatedEntities(Parameters parameters);

        void DeleteEntities(List<EntitySearch> entities);

        void DeleteComments(List<CommentSearch> comments);

        void DeleteComments(List<SimpleSearch> simpleEntities);
    }
}