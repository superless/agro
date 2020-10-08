using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using trifenix.connect.interfaces.search;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.enums;
using trifenix.connect.mdm.search.model;

namespace trifenix.connect.search
{
    public class MainSearch<GeoPointType> : IBaseEntitySearch<GeoPointType>
    {
        private readonly SearchServiceClient _search;
        private readonly string entityIndex;
        private readonly CorsOptions corsOptions;

        public MainSearch(string SearchServiceName, string SearchServiceKey, string entityIndex, CorsOptions corsOptions)
        {
            _search = new SearchServiceClient(SearchServiceName, new SearchCredentials(SearchServiceKey));
            this.entityIndex = entityIndex;
            this.corsOptions = corsOptions;
        }
        /// <summary>
        /// Añade o elimina items dentro de azure search.
        /// </summary>
        /// <typeparam name="T">El tipo solo puede ser una entidad soportada dentro de azure search, se validará que cumpla</typeparam>
        /// <param name="elements">elementos a guardar dentro del search</param>
        /// <param name="operationType">Tipo de operación Añadir o borrar</param>
        private void OperationElements<T>(List<T> elements, SearchOperation operationType)
        {

            // validar que sea un elemento de tipo search.
            var indexName = entityIndex;

            // obtiene el client azure search de acuerdo al índice.
            var indexClient = _search.Indexes.GetClient(indexName);

            // realiza la acción segun el argumento
            var actions = elements.Select(o => operationType == SearchOperation.Add ? IndexAction.Upload(o) : IndexAction.Delete(o));

            // preparando la ejecución
            var batch = IndexBatch.New(actions);

            // ejecución.
            indexClient.Documents.Index(batch);
        }

        /// <summary>
        /// Añade elementos al search.
        /// </summary>
        /// <typeparam name="T">Esto debería ser EntitySearch</typeparam>
        /// <param name="elements"></param>
        public void AddElements(List<IEntitySearch<GeoPointType>> elements)
        {
            OperationElements(elements, SearchOperation.Add);
        }

        /// <summary>
        /// Añade un elemento al search.
        /// </summary>
        /// <typeparam name="T">Esto debería ser EntitySearch</typeparam>
        /// <param name="elements"></param>
        public void AddElement(IEntitySearch<GeoPointType> element)
        {
            OperationElements(new List<IEntitySearch<GeoPointType>> { element }, SearchOperation.Add);
        }

        /// <summary>
        /// Borra elementos desde el search.
        /// </summary>        
        /// <param name="elements">entidades a eliminar</param>
        public void DeleteElements(List<IEntitySearch<GeoPointType>> elements)
        {
            OperationElements(elements, SearchOperation.Delete);
        }

        /// <summary>
        /// filtra elementos del search de acuerdo a una conuslta
        /// </summary>
        /// <param name="filter">filtro de azure (Odata)</param>
        /// <returns>Entidades encontradas</returns>
        public List<IEntitySearch<GeoPointType>> FilterElements(string filter)
        {
            var indexName = entityIndex;
            var indexClient = _search.Indexes.GetClient(indexName);
            var result = indexClient.Documents.Search<EntitySearch>(null, new SearchParameters { Filter = filter });




            return result.Results.Select(v => (IEntitySearch<GeoPointType>)new EntityBaseSearch<GeographyPoint>
            {
                bl = v.Document.bl,
                created = v.Document.created,
                dbl = v.Document.dbl,
                dt = v.Document.dt,
                enm = v.Document.enm,
                geo = v.Document.geo,
                id = v.Document.id,
                index = v.Document.index,
                num32 = v.Document.num32,
                num64 = v.Document.num64,
                rel = v.Document.rel,
                str = v.Document.str,
                sug = v.Document.sug


            }).ToList();
        }
        /// <summary>
        /// Vacía el índice.
        /// </summary>
        public void EmptyIndex()
        {
            var indexName = entityIndex;
            _search.Indexes.Delete(indexName);
            CreateOrUpdateIndex();
        }
        /// <summary>
        /// Crea o actualiza el índice en el azure search.
        /// </summary>
        public void CreateOrUpdateIndex()
        {
            var indexName = entityIndex;
            // creación del índice.
            _search.Indexes.CreateOrUpdate(new Index { Name = indexName, Fields = FieldBuilder.BuildForType<EntitySearch>(), CorsOptions = corsOptions });
        }


        /// <summary>
        /// Borrar elementos de azure search de acuerdo auna consulta
        /// </summary>        
        /// <param name="query">consulta de elementos a eliminar</param>
        public void DeleteElements(string query)
        {
            var elements = FilterElements(query);
            if (elements.Any())
                DeleteElements(elements);
        }
    }
}
