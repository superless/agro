using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.external.interfaces;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro_model_input;
using trifenix.connect.mdm.entity_model;

namespace trifenix.agro.search.interfaces {

    public interface IAgroSearch<T> {

        void AddElements(List<IEntitySearch<T>> elements);

        void AddElement(IEntitySearch<T> element);

        void DeleteElements(List<IEntitySearch<T>> elements);

        void DeleteElements(string query);

        void EmptyIndex();

        Task GenerateIndex(IAgroManager<T> agro);

        Task RegenerateIndex(IAgroManager<T> agro);

        List<IEntitySearch<T>> FilterElements(string filter);

        IEntitySearch<T> GetEntity(EntityRelated entityRelated, string id);

        void DeleteEntity(EntityRelated entityRelated, string id);

        void DeleteElementsWithRelatedElement(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement);

        IEntitySearch<T>[] GetElementsWithRelatedElement(EntityRelated elementToGet, EntityRelated relatedElement, string idRelatedElement);

        void DeleteElementsWithRelatedElementExceptId(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement, string elementExceptId);

        IEntitySearch<T>[] GetEntitySearch<T2>(T2 model) where T2 : DocumentBase;

        IEntitySearch<T>[] GetEntitySearchByInput<T2>(T2 model) where T2 : InputBase;

        void AddDocument<T2>(T2 document) where T2:DocumentBase;

    }

}