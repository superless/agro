using res.model.booleans;
using res.model.dates;
using res.model.doubles;
using res.model.entities;
using res.model.enums;
using res.model.geos;
using res.model.nums;
using res.model.strings;
using System;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.attr.resources
{
    public class DisplaySearchEnum : DisplaySearchAttribute
    {
        public DisplaySearchEnum(EnumRelated index) : base(ResourcesExtension.GetResourceCollection(ResourceRelated.ENUM),  ResourceRelated.ENUM)
        {
            Index = index;
        }

        public EnumRelated Index { get; }

        protected override string GetIndexName()
        {
            return Enum.GetName(typeof(EnumRelated), index);
        }
    }

    public class DisplaySearchString : DisplaySearchAttribute
    {
        public DisplaySearchString(StringRelated index) : base(ResourcesExtension.GetResourceCollection(ResourceRelated.STR),  ResourceRelated.STR)
        {
            Index = index;
        }

        public StringRelated Index { get; }

        protected override string GetIndexName()
        {
            return Enum.GetName(typeof(StringRelated), index);
        }
    }

    public class DisplaySearchNum : DisplaySearchAttribute
    {
        public DisplaySearchNum(NumRelated index) : base(ResourcesExtension.GetResourceCollection(ResourceRelated.NUM), ResourceRelated.NUM)
        {
            Index = index;
        }

        public NumRelated Index { get; }

        protected override string GetIndexName()
        {
            return Enum.GetName(typeof(NumRelated), index);
        }
    }

    public class DisplaySearchBool : DisplaySearchAttribute
    {
        public DisplaySearchBool(BoolRelated index) : base(ResourcesExtension.GetResourceCollection(ResourceRelated.BOOL), ResourceRelated.BOOL)
        {
            Index = index;
        }

        public BoolRelated Index { get; }

        protected override string GetIndexName()
        {
            return Enum.GetName(typeof(BoolRelated), index);
        }
    }

    public class DisplaySearchDate : DisplaySearchAttribute
    {
        public DisplaySearchDate(DateRelated index) : base(ResourcesExtension.GetResourceCollection(ResourceRelated.DATE), ResourceRelated.DATE)
        {
            Index = index;
        }

        public DateRelated Index { get; }

        protected override string GetIndexName()
        {
            return Enum.GetName(typeof(DateRelated), index);
        }
    }

    public class DisplaySearchGeo : DisplaySearchAttribute
    {
        public DisplaySearchGeo(GeoRelated index) : base(ResourcesExtension.GetResourceCollection(ResourceRelated.GEO), ResourceRelated.GEO)
        {
            Index = index;
        }

        public GeoRelated Index { get; }

        protected override string GetIndexName()
        {
            return Enum.GetName(typeof(GeoRelated), index);
        }
    }


    public class DisplaySearchDbl : DisplaySearchAttribute
    {
        public DisplaySearchDbl(DoubleRelated index) : base(ResourcesExtension.GetResourceCollection(ResourceRelated.DBL), ResourceRelated.DBL)
        {
            Index = index;
        }

        public DoubleRelated Index { get; }

        protected override string GetIndexName()
        {
            return Enum.GetName(typeof(DoubleRelated), index);
        }
    }

    public class DisplaySearchEntity : DisplaySearchAttribute
    {
        public DisplaySearchEntity(EntityRelated index) : base(ResourcesExtension.GetResourceCollection(ResourceRelated.REF), ResourceRelated.REF)
        {
            Index = index;
        }

        public EntityRelated Index { get; }

        protected override string GetIndexName()
        {
            return Enum.GetName(typeof(EntityRelated), index);
        }
    }



    public static class ResourcesExtension {

        
    }
}
