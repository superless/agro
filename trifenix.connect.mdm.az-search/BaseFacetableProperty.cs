using Microsoft.Azure.Search;
using Newtonsoft.Json;
using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.mdm.az_search
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
        [IsFacetable]
        [JsonProperty("facet")]
        public string facet { get => $"{index},{value}"; }
    }

    
}
