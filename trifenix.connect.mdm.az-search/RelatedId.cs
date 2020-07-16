using Microsoft.Azure.Search;
using Newtonsoft.Json;
using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.mdm.az_search
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
        [IsFilterable]
        [JsonProperty("entityIndex")]
        public int index { get; set; }


        /// <summary>
        /// identificador de una entidad
        /// </summary>
        [IsFilterable]
        [JsonProperty("entityId")]
        public string id { get; set; }


        /// <summary>
        /// facet que incluye el identificador de la entidad y la id para busquedas agrupadas
        /// en azure search.
        /// </summary>
        [IsFacetable]
        [JsonProperty("facet")]
        public string facet { get => $"{index},{id}"; }
    }
}
