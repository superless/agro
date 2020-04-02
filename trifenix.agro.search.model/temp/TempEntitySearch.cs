using Microsoft.Azure.Search;
using Microsoft.Spatial;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.search.model.temp {

    public class EntitySearchV2 {

        [Key]
        [IsFilterable]
        public string Id { get; set; }

        [IsFilterable, IsFacetable]
        public int EntityIndex { get; set; }

        [IsSortable]
        public DateTime Created { get; set; }

        [JsonProperty("rel")]
        public RelatedId[] RelatedIds { get; set; }

        [JsonProperty("sug")]
        public SuggestProperty[] SuggestProperties { get; set; }

        [JsonProperty("str")]
        public StrProperty[] StringProperties { get; set; }

        [JsonProperty("enum")]
        public EnumProperty[] EnumProperties { get; set; }

        [JsonProperty("num32")]
        public Num32Property[] NumProperties { get; set; }

        [JsonProperty("num64")]
        public Num64Property[] Num64Properties { get; set; }

        [JsonProperty("dbl")]
        public DblProperty[] DoubleProperties { get; set; }

        [JsonProperty("dt")]
        public DtProperty[] DtProperties { get; set; }

        [JsonProperty("geo")]
        public GeoProperty[] GeoProperties { get; set; }

        [JsonProperty("bl")]
        public BoolProperty[] BoolProperties { get; set; }

    }

    public class RelatedId {
        [IsFilterable]
        public int EntityIndex { get; set; }
        [IsFilterable]
        public string EntityId { get; set; }
        [IsFacetable]
        public string Id { get => $"{EntityIndex},{EntityId}"; }
    }

    public class BaseProperty<T> {
        [IsFilterable]
        public int PropertyIndex { get; set; }
        [IsFilterable]
        public virtual T Value { get; set; }
    }

    public class BaseFacetableProperty<T> : BaseProperty<T> {
        [IsFacetable]
        public string Id { get => $"{PropertyIndex},{Value}"; }
    }

    public class SuggestProperty : BaseProperty<string> {
        [IsFilterable, IsSearchable]
        public override string Value { get; set; }
    }

    public class StrProperty : BaseFacetableProperty<string> {
        [IsFilterable, IsSearchable]
        public override string Value { get; set; }
    }

    public class EnumProperty : BaseFacetableProperty<int> { }

    public class Num32Property : BaseProperty<int> { }

    public class Num64Property : BaseProperty<long> { }

    public class DblProperty : BaseProperty<double> { }

    public class DtProperty : BaseProperty<DateTime> { }

    public class BoolProperty : BaseProperty<bool> { }

    public class GeoProperty : BaseProperty<GeographyPoint> { }

    public class GeoPointTs {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    
    public class GeographyProperty {   
        public int PropertyIndex { get; set; }
        public GeoPointTs Value { get; set; }
    }

}