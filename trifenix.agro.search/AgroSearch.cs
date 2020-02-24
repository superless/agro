using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using System.Linq;
using trifenix.agro.enums;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.search.model.@base;

namespace trifenix.agro.search.operations {
    public class AgroSearch : IAgroSearch{

        private readonly SearchServiceClient _search;
        private readonly string _entityIndex = "entities";
        private readonly string _commentIndex = "comments";
        private readonly string _simpleEntityIndex = "simpleEntity";


        public AgroSearch(string SearchServiceName, string SearchServiceKey) {
            _search = new SearchServiceClient(SearchServiceName, new SearchCredentials(SearchServiceKey));
            if (_search.Indexes.Exists(_entityIndex))
                return;
            _search.Indexes.CreateOrUpdate(new Index { Name = _entityIndex, Fields = FieldBuilder.BuildForType<EntitySearch>() });
            if (_search.Indexes.Exists(_commentIndex))
                return;
            _search.Indexes.CreateOrUpdate(new Index { Name = _commentIndex, Fields = FieldBuilder.BuildForType<CommentSearch>() });
        }

        private void OperationElements<T>(string indexName, List<T> elements, SearchOperation operation) where T : BaseSearch {
            var indexClient = _search.Indexes.GetClient(indexName);
            var actions = elements.Select(o => operation == SearchOperation.Add?IndexAction.MergeOrUpload(o): IndexAction.Delete(o));
            var batch = IndexBatch.New(actions);
            indexClient.Documents.Index(batch);
        }

        public void AddEntities(List<EntitySearch> entities) {
            OperationElements(_entityIndex, entities, SearchOperation.Add);
        }

        public void AddSimpleEntities(List<SimpleSearch> simpleEntities)
        {
            OperationElements(_simpleEntityIndex, simpleEntities, SearchOperation.Add);
        }

        public void DeleteComments(List<SimpleSearch> simpleEntities)
        {
            OperationElements(_simpleEntityIndex, simpleEntities, SearchOperation.Delete);
        }



        public void DeleteEntities(List<EntitySearch> entities) {
            OperationElements(_entityIndex, entities, SearchOperation.Delete);
        }

        public void AddComments(List<CommentSearch> comments)
        {
            OperationElements(_commentIndex, comments, SearchOperation.Add);
        }

        public void DeleteComments(List<CommentSearch> comments)
        {
            OperationElements(_commentIndex, comments, SearchOperation.Delete);
        }


        // TODO: search eliminar, es directo desde js
        #region deleteSearch

        public EntitiesSearchContainer GetPaginatedEntities(Parameters parameters)
        {
            var SearchParameters = new SearchParameters
            {
                Filter = parameters.Filters.ToString(),
                SearchFields = new[] { "IdentificadorDeEntidad" },
                Top = parameters.Quantity,
                IncludeTotalResultCount = true
            };
            if (parameters.Page.HasValue && parameters.Quantity.HasValue)
            {
                int? skip = (parameters.Page - 1) * parameters.Quantity;
                SearchParameters.Skip = skip;
            }
            if (parameters.Desc.HasValue)
            {
                string order = parameters.Desc.Value ? "asc" : "desc";
                SearchParameters.OrderBy = new[] { $"IdentificadorDeEntidad {order}" };
            }
            if (string.IsNullOrWhiteSpace(parameters.TextToSearch))
                parameters.TextToSearch = null;
            EntitiesSearchContainer entitySearch = GetSearch(parameters.TextToSearch, SearchParameters);
            return entitySearch;
        }


        
        private EntitiesSearchContainer GetSearch(string search, SearchParameters parameters)
        {
            var indexClient = _search.Indexes.GetClient(_entityIndex);
            var result = indexClient.Documents.Search<EntitySearch>(search, parameters);
            return new EntitiesSearchContainer
            {
                Total = result.Count ?? 0,
                Entities = result.Results.Select(v => v.Document).ToArray()
            };
        }

        

        #endregion


    }


    

}