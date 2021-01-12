using Azure.Search.Documents.Indexes;
using Newtonsoft.Json;
using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.mdm.search.model
{

    /// <summary>
    /// Implementación para azure de una propiedad para entidades relacionadas
    /// con un entity search.
    /// </summary>
    public class RelatedId : IRelatedId
    {
        /// <summary>
        /// índice de una entidad
        /// </summary>
        [SimpleField(IsKey = false, IsFilterable = true)]
        [JsonProperty("index")]
        public int index { get; set; }


        /// <summary>
        /// identificador de una entidad
        /// </summary>
        [SimpleField(IsKey = false, IsFilterable = true)]
        [JsonProperty("id")]
        public string id { get; set; }


        /// <summary>
        /// facet que incluye el identificador de la entidad y la id para busquedas agrupadas
        /// en azure search.
        /// </summary>
        [SimpleField(IsKey = false, IsFacetable = true)]
        [JsonProperty("facet")]
        public string facet { get => $"{index},{id}"; }
    }
}
