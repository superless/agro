using Azure.Search.Documents.Indexes;
using Newtonsoft.Json;
using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.mdm.search.model
{
    /// <summary>
    /// Implementación para azure search
    /// con el elemento base para propiedades que incluyen
    /// facets, como enumeraciones, entidades y fechas.
    /// </summary>
    /// <typeparam name="T">Tipo de valor que tiene asignado la propiedad (bool, num, etc.)</typeparam>
    public class BaseFacetableProperty<T> : BaseProperty<T>, IPropertyBaseFaceTable<T>
    {

        /// <summary>
        /// facet, ver interface para más documentación
        /// </summary>
        [SimpleField(IsKey = false, IsFacetable = true)]
        [JsonProperty("facet")]
        public string facet { get => $"{index},{value}"; }
    }

    
}
