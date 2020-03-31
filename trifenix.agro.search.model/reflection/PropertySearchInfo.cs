using System.Collections.Generic;

using trifenix.agro.enums.searchModel;

namespace trifenix.agro.search.model.reflection
{
    public class PropertySearchInfo {
        public Related Related { get; set; }
        public int Index { get; set; }

        public string Name { get; set; }
        public int IndexClass { get; set; }
        public Dictionary<int, string> Enums { get; set; }


        public bool IsEnumerable { get; set; }

        public string Description { get; set; }

        public string Display { get; set; }

        public string PlaceHolder { get; set; }

        public bool Required { get; set; }

        public bool Unique { get; set; }

        public bool isRut { get; set; }

        public bool isEmail { get; set; }

        public int? minLength { get; set; }

        public int? maxLength { get; set; }

        public GroupInput Group { get; set; }


    }

    public class GroupInput {
        public int Index { get; set; }

        public string Name { get; set; }

        public Device Device { get; set; }




    }

}