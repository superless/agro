using System;
using System.Collections.Generic;
using trifenix.agro.enums.searchModel;
using trifenix.agro.search.model.reflection;

namespace trifenix.agro.search.model.ts
{
    public class Result
    {
        public long Total { get; set; }

        public EntitySearch[] Entities { get; set; }

        public int Current { get; set; }

        public Facet[] Facets { get; set; }

        /// <summary>
        /// filter
        /// </summary>
        public FilterModel Filter { get; set; }

        public Related? EntityKindSort { get; set; }

        public bool? ByDesc { get; set; }

        public int? IndexSorted { get; set; }

        public int IndexPropName { get; set; }
    }


    public class FilterGlobalInput {
        public EntityRelated indexMain { get; set; }

        //clase para almacenar el resultado de los filtros globales.
    }

    public class Facet {
        public int Index { get; set; }

        public string Title { get; set; }

        public string Value { get; set; }

        public long Count { get; set; }
    }




    

    public class EnumDictionary : DefaultDictionary
    {

        
        public Dictionary<int, string> EnumData { get; set; }
        
    }

    public class DefaultDictionary
    {

        public string NameProp { get; set; }

        

        public bool isArray { get; set; }

        public EntitySearchDisplayInfo Info { get; set; }

        public bool Required { get; set; }

        public bool Unique { get; set; }

        public bool Visible { get; set; }

        public bool AutoNumeric { get; set; }

        public bool HasInput { get; set; }

    }

    public class ReletadIdTs : RelatedId {

        public string Name { get; set; }
    }


    

    public class ModelMetaData {
        

        public ModelDictionary[] Indexes { get; set; }
    }


    

    public class ModelDictionary {

        public string Title { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }

        public EntityRelated Index { get; set; }


        public bool Visible { get; set; }

        public EntityKind EntityKind { get; set; }

        public string PathName { get; set; }

        public bool AutoNumeric { get; set; }

        public string ClassName { get; set; }


        public Dictionary<int, DefaultDictionary> StringData { get; set; }

        public Dictionary<int, DefaultDictionary> NumData { get; set; }


        public Dictionary<int, DefaultDictionary> DoubleData { get; set; }


        public Dictionary<int, DefaultDictionary> BoolData { get; set; }

        public Dictionary<int, DefaultDictionary> GeoData { get; set; }

        public Dictionary<int, DefaultDictionary> DateData { get; set; }

        public Dictionary<int, EnumDictionary> EnumData { get; set; }


        public Dictionary<int, DefaultDictionary> relData { get; set; }

    }

    public class FilterModel {
        public Dictionary<int, FilterBase<string>[]> FilterStr { get; set; }
        public Dictionary<int, FilterBase<string>[]> FilterEntity { get; set; }

        public Dictionary <int, FilterBase<int>[]> EnumFilter { get; set; }

        public Dictionary<int, FilterBase<long>[]> LongFilter { get; set; }

        public Dictionary <int, FilterBase<int>[]> NumFilter { get; set; }

        public Dictionary<int, FilterBase<bool>[]> BoolFilters { get; set; }

        public Dictionary<int, FilterBase<DateTime>[]> DateFilters { get; set; }

        public Dictionary<int, FilterBase<double>[]> DoubleFilters { get; set; }




    }


    public class FilterBase<T> {        

        public FilterType FilterType { get; set; }

        public T Value { get; set; }


    }

   

    


}
