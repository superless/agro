using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.search.model.temp;
using TypeGen.Core.TypeAnnotations;

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
        SEARCH =0,
        SELECTED = 1,
        SELECTED_GROUP=2

    }

    


}
