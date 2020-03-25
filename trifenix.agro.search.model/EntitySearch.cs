using Microsoft.Azure.Search;
using Microsoft.Spatial;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.search.model {

    public class EntitySearch {

        [Key]
        [IsFilterable]
        public string Id { get; set; }                              

        [IsFilterable, IsFacetable]
        public int EntityIndex { get; set; }                        

        [IsSortable]
        public DateTime Created { get; set; }

        [JsonProperty("prop_ids")]
        public Reference[] References { get; set; }

        [JsonProperty("prop_str")]
        public String[] Strings { get; set; }

        [JsonProperty("prop_num")]
        public Num32[] Integers { get; set; }

        [JsonProperty("prop_num64")]
        public Num64[] Longs { get; set; }

        [JsonProperty("prop_dbl")]
        public Double[] Doubles { get; set; }

        [JsonProperty("prop_bool")]
        public Bool[] Bools { get; set; }

        [JsonProperty("prop_geo")]
        public Geo[] GeoPoints { get; set; }

        [JsonProperty("prop_dt")]
        public Date[] Dates { get; set; }


        [JsonProperty("prop_sug")]
        public Suggest[] Suggests { get; set; }

        public RelatedEnumValue[] RelatedEnumValues { get; set; }

    }

    //public class RelatedId {

    //    [IsFilterable, IsFacetable]
    //    public int EntityIndex { get; set; }

    //    [IsFilterable]
    //    public string EntityId { get; set; }

    //    [IsFilterable, IsFacetable]
    //    public string Id { get { return $"{EntityIndex},{EntityId}"; } }

    //}

    //public class Property {

    //    [IsFilterable, IsFacetable]
    //    public int PropertyIndex { get; set; }

    //    [IsSearchable]
    //    public string Value { get; set; }

    //    [IsFilterable, IsFacetable]
    //    public string Id { get { return $"{PropertyIndex},{Value}";  } }

    //}

    public class RelatedEnumValue {

        [IsFilterable, IsFacetable]
        public int EnumerationIndex { get; set; }

        [IsFilterable]
        public int Value { get; set; }

        [IsFilterable, IsFacetable]
        public string Id { get { return $"{EnumerationIndex},{Value}"; } }
    }

    public class Reference {

        [IsFilterable, IsFacetable]
        public int EntityTypeIndex { get; set; }

        [IsFilterable]
        public string IdReference { get; set; }
    }

    public class String {

        [IsFilterable, IsFacetable]
        public int PropertyIndex { get; set; }

        [IsSearchable, IsFilterable]
        public string Value { get; set; }
    }

    public class Num32 {

        [IsFilterable, IsFacetable]
        public int PropertyIndex { get; set; }

        [IsFilterable, IsSortable]
        public int Value { get; set; }
    }

    public class Num64 {

        [IsFilterable, IsFacetable]
        public int PropertyIndex { get; set; }

        [IsFilterable, IsSortable]
        public long Value { get; set; }
    }

    public class Double {

        [IsFilterable, IsFacetable]
        public int PropertyIndex { get; set; }

        [IsFilterable, IsSortable]
        public double Value { get; set; }
    }

    public class Bool {

        [IsFilterable, IsFacetable]
        public int PropertyIndex { get; set; }

        [IsFilterable]
        public bool Value { get; set; }
    }

    public class Geo {

        public int PropertyIndex { get; set; }
        public GeographyPoint Value { get; set; }
    }

    public class Date {

        [IsFilterable, IsFacetable]
        public int PropertyIndex { get; set; }

        [IsFilterable, IsSortable]
        public DateTime Value { get; set; }
    }

    public class Suggest {
        public int PropertyIndex { get; set; }
        public string Value { get; set; }
    }

}