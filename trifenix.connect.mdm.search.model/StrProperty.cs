using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Newtonsoft.Json;
using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.mdm.search.model
{

    /// <summary>
    /// Propiedad de un string,
    /// mantiene un índice y un valor de tipo string.
    /// </summary>
    public class StrProperty : BaseProperty<string>, IStrProperty
    {

        /// <summary>
        /// se sobrescribe la propiedad valor, dado que a diferencia del base
        /// string debe incluír busqueda,
        /// revisar baseProperty para ver valor original.
        /// </summary>
        [SearchableField(IsKey =false, IsFilterable = true)]
        [JsonProperty("value")]
        public override string value { get; set; }
    }
}
