using System.Collections.Generic;
using trifenix.agro.db;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.model_input;
using trifenix.connect.mdm.az_search;
using trifenix.connect.mdm.entity_model;

namespace trifenix.agro.search.interfaces
{
    public interface IAgroSearch {

        void AddElements(List<EntitySearch> elements);

        void AddElement(EntitySearch element);

        void DeleteElements(List<EntitySearch> elements);

        void DeleteElements(string query);

        void EmptyIndex<IndexSearch>(string indexName);

        

        List<EntitySearch> FilterElements(string filter);

        EntitySearch GetEntity(EntityRelated entityRelated, string id);

        void DeleteEntity(EntityRelated entityRelated, string id);

        void DeleteElementsWithRelatedElement(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement);

        EntitySearch[] GetElementsWithRelatedElement(EntityRelated elementToGet, EntityRelated relatedElement, string idRelatedElement);

        void DeleteElementsWithRelatedElementExceptId(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement, string elementExceptId);

        EntitySearch GetEntitySearch<T>(T model) where T : DocumentBase;

        EntitySearch GetEntitySearchByInput<T>(T model) where T : InputBase;
        void AddDocument<T>(T document) where T:DocumentBase;
    }
}