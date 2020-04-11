using System.Collections.Generic;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.search.model.reflection {

    public class PropertySearchInfo {
        public int Index { get; set; }
        public Related Related { get; set; }
        public string Name { get; set; }
        public int IndexClass { get; set; }
        public Dictionary<int, string> Enums { get; set; }
        public bool IsEnumerable { get; set; }
        public bool IsRequired { get; set; }
        public bool IsUnique { get; set; }
        public int? MaxLength { get; set; }
        public int? MinLength { get; set; }
        public GroupInput[] Group { get; set; }
        public EntitySearchDisplayInfo Info { get; set; }
    }

    public class EntitySearchDisplayInfo {
        public string Title { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string Column { get; set; }
    }

    public class GroupInput {
        public int Index { get; set; }
        public string Title { get; set; }
        public Device Device { get; set; }
    }

}