using System.Collections.Generic;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.search.model.ts
{
    public class Result
    {
        public long Total { get; set; }

        public EntitySearch[] Entities { get; set; }

        public int Current { get; set; }
    }


    public class SearchType {
        public TypeEntity EntityType { get; set; }

        public string Name { get; set; }


        public int MaxOptions { get; set; }

        public bool Default { get; set; }


        public int MainEntityIndex { get; set; }


        public int EntitySearchTypeIndex { get; set; }


        public bool DataDependant { get; set; }


        public int PropertyIndex { get; set; }


        public int PropertyCategoryIndex { get; set; }


        public int CategoryIndex { get; set; }


        public string MessageNotFound { get; set; }


        public string PlaceHolder { get; set; }
    }



    public enum TypeEntity {
        SEARCH = 0,
        SELECTED = 1,
        SELECTED_GROUP = 2

    }



    

    public class EnumDictionary : DefaultDictionary
    {

        
        public Dictionary<int, string> EnumData { get; set; }
        
    }

    public class DefaultDictionary
    {

        public string NameProp { get; set; }

        public string Description { get; set; }

        public bool isArray { get; set; }


    }

    public class ReletadIdTs : RelatedId {

        public string Name { get; set; }
    }

    public class ModelByIndex {

        public Dictionary<int, ModelDictionary> Indexes { get; set; }
    }

    public class ModelDictionary {

        public Dictionary<int, DefaultDictionary> RelatedData { get; set; }

        public Dictionary<int, DefaultDictionary> StringData { get; set; }

        public Dictionary<int, DefaultDictionary> NumData { get; set; }


        public Dictionary<int, DefaultDictionary> DoubleData { get; set; }


        public Dictionary<int, DefaultDictionary> BoolData { get; set; }

        public Dictionary<int, DefaultDictionary> GeoData { get; set; }

        public Dictionary<int, DefaultDictionary> DateData { get; set; }

        public Dictionary<int, EnumDictionary> EnumData { get; set; }

        



    }

    


}
