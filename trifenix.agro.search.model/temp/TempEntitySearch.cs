using Microsoft.Azure.Search;
using Microsoft.Spatial;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.search.model.temp {

    public class EntitySearch
    {
        [Key]
        [IsFilterable]
        public string Id { get; set; }

        

        [IsFilterable, IsFacetable]
        public int EntityIndex { get; set; }

        [IsSortable]
        
        public DateTime Created { get; set; }

        [JsonProperty("prop_rel")]
        public RelatedId[] RelatedIds { get; set; }


        [JsonProperty("prop_sug")]
        public SuggestProperty[] SuggestProperties { get; set; }

        [JsonProperty("prop_str")]
        public StrProperty[] StringProperties { get; set; }

        [JsonProperty("prop_enum")]
        public EnumProperty[] EnumProperties { get; set; }


        [JsonProperty("prop_num32")]
        public Num32Property[] NumProperties { get; set; }

        [JsonProperty("prop_num64")]
        public Num64Property[] Num64Properties { get; set; }

        [JsonProperty("prop_dbl")]
        public DblProperty[] DoubleProperties { get; set; }


        [JsonProperty("prop_dt")]
        public DtProperty[] DtProperties { get; set; }


        [JsonProperty("prop_geo")]
        public GeoProperty[] GeoProperties { get; set; }





    }


    public class BaseProperty<T> {

        [IsFilterable]
        public int PropertyIndex { get; set; }

        [IsSearchable, IsFilterable]
        public T Value { get; set; }

    }

    public class RelatedId {


        [IsFilterable]
        public int EntityIndex { get; set; }

        [IsFilterable]
        public string EntityId { get; set; }

        [IsFacetable]
        public string Id { get { return $"{EntityIndex},{EntityId}"; } }
    }

    public class StrProperty : BaseProperty<string>
    {


        [IsFacetable]
        public string Id { get { return $"{PropertyIndex},{Value}"; } }
    }

    public class EnumProperty : BaseProperty<int>
    {

        [IsFacetable]
        public string Id { get { return $"{PropertyIndex},{Value}"; } }
    }

    public class SuggestProperty : BaseProperty<string>
    {
    }



    public class Num32Property : BaseProperty<int>
    {
    }

    public class Num64Property : BaseProperty<long>
    {
    }

    public class DblProperty : BaseProperty<double>
    {

    }

    public class DtProperty : BaseProperty<DateTime>
    {
    }

    public class BoolProperty : BaseProperty<bool>
    {
    }

    public class GeoProperty : BaseProperty<GeographyPoint>
    {

    }

    public class GeoPointTs {
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }
    }




}
