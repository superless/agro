using System;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.interfaces.log;
using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.agro.interfaces.search
{
    public interface IRelatedSearch<T> : ILogSimpleQuery
    {
        IEntitySearch<T> GetEntity(EntityRelated entityRelated, string id);

        void DeleteEntity(EntityRelated entityRelated, string id);

        void DeleteElementsWithRelatedElement(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement);

        IEntitySearch<T>[] GetElementsWithRelatedElement(EntityRelated elementToGet, EntityRelated relatedElement, string idRelatedElement);

        void DeleteElementsWithRelatedElementExceptId(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement, string elementExceptId);
    }

    
}
