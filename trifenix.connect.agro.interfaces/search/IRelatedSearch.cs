using System;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.interfaces.log;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.model;

namespace trifenix.connect.agro.interfaces.search
{
 
    /// <summary>
    /// Operaciones de 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRelatedAgroSearch<T> : ILogSimpleQuery
    {
        IEntitySearch<T> GetEntity(EntityRelated entityRelated, string id);

        void DeleteEntity(EntityRelated entityRelated, string id);

        void DeleteElementsWithRelatedElement(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement);

        IEntitySearch<T>[] GetElementsWithRelatedElement(EntityRelated elementToGet, EntityRelated relatedElement, string idRelatedElement);

        void DeleteElementsWithRelatedElementExceptId(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement, string elementExceptId);

        /// <summary>
        /// Obtiene un entitySearch desde un elemento de la base de datos.
        /// </summary>
        /// <typeparam name="T2">elemento de la base de datos</typeparam>
        /// <param name="model"></param>
        /// <returns>una colección de entitySearch que representa un elemento de la base de datos</returns>
        IEntitySearch<T>[] GetEntitySearch<T2>(T2 model) where T2 : DocumentDb;

        /// <summary>
        /// Añade un elemento de la base de datos de busqueda
        /// para esto primero debe convertir la entidad de base de datos en uno o más entitySearchs.
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="document"></param>
        void AddDocument<T2>(T2 document) where T2 : DocumentDb;
    }

    
}
