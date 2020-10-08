using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using trifenix.connect.agro.external.helper;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro.interfaces.search;
using trifenix.connect.entities.cosmos;
using trifenix.connect.external;
using trifenix.connect.input;
using trifenix.connect.interfaces.external;
using trifenix.connect.interfaces.search;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.search;

namespace trifenix.connect.agro.external
{
    /// <summary>
    /// Clase que interactua con azure search
    /// importante! es muy probable que los comentarios sean eliminados 
    /// refactorizar esta clase para soportar mejor el modelo.
    /// </summary>
    public class AgroSearch<GeoPointType> : IAgroSearch<GeoPointType> 
    {
    

        

        // índice para las entidades, nombre del indice en azure
        private readonly string _entityIndex = "entitiesv2";


        private IRelatedSearch<GeoPointType> relatedSearch;

        private IBaseEntitySearch<GeoPointType> mainSearch;

        private IEntitySearchOper<GeoPointType> operEntitySearch;



        /// <summary>
        /// Consultas y mutación en Azure search.
        /// </summary>
        /// <param name="SearchServiceName">nombre del servicio</param>
        /// <param name="SearchServiceKey">clave del servicio</param>
        /// <param name="corsOptions">opciones de cors</param>
        public AgroSearch(string SearchServiceName, string SearchServiceKey, CorsOptions corsOptions)
        {
            mainSearch = new MainSearch<GeoPointType>(SearchServiceName, SearchServiceKey, _entityIndex, corsOptions);

            relatedSearch = new SearchQueryOperations<GeoPointType>(mainSearch);

            operEntitySearch = new EntitySearchMgmt<GeoPointType>(mainSearch);
        }


        public AgroSearch(IRelatedSearch<GeoPointType> relatedSearch, IBaseEntitySearch<GeoPointType> mainSearch, IEntitySearchOper<GeoPointType> operEntitySearch)
        {
            this.mainSearch = mainSearch;

            this.relatedSearch = relatedSearch;

            this.operEntitySearch = operEntitySearch;
        }


        public void AddDocument<T2>(T2 document) where T2 : DocumentBase
        {
            operEntitySearch.AddDocument(document);
        }

        public void AddElement(IEntitySearch<GeoPointType> element)
        {
            mainSearch.AddElement(element);
        }

        public void AddElements(List<IEntitySearch<GeoPointType>> elements)
        {
            mainSearch.AddElements(elements);
        }


        public void CreateOrUpdateIndex()
        {
            mainSearch.CreateOrUpdateIndex();
        }

        public void DeleteElements(List<IEntitySearch<GeoPointType>> elements)
        {
            mainSearch.DeleteElements(elements);
        }

        public void DeleteElements(string query)
        {
            mainSearch.DeleteElements(query);
        }

        public void DeleteElementsWithRelatedElement(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement)
        {
            relatedSearch.DeleteElementsWithRelatedElement(elementToDelete, relatedElement, idRelatedElement);
        }

        public void DeleteElementsWithRelatedElementExceptId(EntityRelated elementToDelete, EntityRelated relatedElement, string idRelatedElement, string elementExceptId)
        {
            relatedSearch.DeleteElementsWithRelatedElementExceptId(elementToDelete, relatedElement, idRelatedElement, elementExceptId);
        }

        public void DeleteEntity(EntityRelated entityRelated, string id)
        {
            relatedSearch.DeleteEntity(entityRelated, id);
        }

        public void EmptyIndex()
        {
            mainSearch.EmptyIndex();
        }

        public List<IEntitySearch<GeoPointType>> FilterElements(string filter)
        {
            return mainSearch.FilterElements(filter);
        }

        public IEntitySearch<GeoPointType>[] GetElementsWithRelatedElement(EntityRelated elementToGet, EntityRelated relatedElement, string idRelatedElement)
        {
            return relatedSearch.GetElementsWithRelatedElement(elementToGet, relatedElement, idRelatedElement);
        }

        public IEntitySearch<GeoPointType> GetEntity(EntityRelated entityRelated, string id)
        {
            return relatedSearch.GetEntity(entityRelated, id);
        }

        public IEntitySearch<GeoPointType>[] GetEntitySearch<T2>(T2 model) where T2 : DocumentBase
        {
            return operEntitySearch.GetEntitySearch(model);
        }

        public IEntitySearch<GeoPointType>[] GetEntitySearchByInput<T2>(T2 model) where T2 : InputBase
        {
            return operEntitySearch.GetEntitySearchByInput(model);
        }

        // consultas 




        












    }

}