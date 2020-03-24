using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.enums;

namespace trifenix.agro.attr
{

    public class SearchModelAttribute : Attribute {
        private readonly EntityRelated _index;
        public SearchModelAttribute(EntityRelated index, Related related)
        {
            _index = index;
            Related = related;
        }

        public int Index => (int)_index;

        public Related Related { get; }


    }

    public enum Related { 
        REFERENCE= 0,
        STR = 1,
        SUGGESTION=2,
        NUM64 = 3,
        NUM32 = 4,
        DBL = 5,
        BOOL = 6,
        GEO_LAT = 7,
        GEO_LONG = 7,

    }
    


}
