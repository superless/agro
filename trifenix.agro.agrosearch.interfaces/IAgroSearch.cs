using System.Collections.Generic;
using trifenix.agro.enums;
using trifenix.agro.search.model;

namespace trifenix.agro.search.interfaces
{
    public interface IAgroSearch {

        void AddElements<T>(List<T> elements);
        void DeleteElements<T>(List<T> elements);

        void DeleteElements<T>(string query);

        
        List<T> FilterElements<T>(string filter);

        EntitySearch GetEntity(EntityRelated entityRelated, string id);

        void DeleteEntity(EntityRelated entityRelated, string id);

        void DeleteElementsWithRelatedElement(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement);

        void DeleteElementsWithRelatedElementExceptId(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement, string elementExceptId);

    }
}