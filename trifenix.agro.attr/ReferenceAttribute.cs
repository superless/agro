using System;
using trifenix.agro.enums;
using trifenix.agro.enums.searchModel;

namespace trifenix.agro.attr {

    public class SearchAttribute : Attribute
    {

        public virtual Related Related { get; }
        public virtual int Index { get; }
    }



    public class ReferenceSearchAttribute : SearchAttribute
    {
        private readonly EntityRelated _index;
        public bool Local { get; }
        public ReferenceSearchAttribute(EntityRelated index, bool local = false) {
            _index = index;
            Local = local;
        }

        
        public override int Index => (int)_index;
        public override Related Related => Local ? Related.REFERENCE : Related.LOCAL_REFERENCE;
    }

    public class StringSearchAttribute : SearchAttribute {
        private readonly StringRelated _index;
        public StringSearchAttribute(StringRelated index) {
            _index = index;
        }
        public override int Index => (int)_index;
        public override Related Related => Related.STR;
    }

    public class SuggestSearchAttribute : SearchAttribute {
        private readonly StringRelated _index;
        public SuggestSearchAttribute(StringRelated index) {
            _index = index;
        }
        public override int Index => (int)_index;
        public override Related Related => Related.SUGGESTION;
    }

    public class Num32SearchAttribute : SearchAttribute {
        private readonly NumRelated _index;
        public Num32SearchAttribute(NumRelated index) {
            _index = index;
        }
        public override int Index => (int)_index;
        public override Related Related => Related.NUM32;
    }

    public class Num64SearchAttribute : SearchAttribute {
        private readonly NumRelated _index;
        public Num64SearchAttribute(NumRelated index) {
            _index = index;
        }
        public override int Index => (int)_index;
        public override Related Related => Related.NUM64;
    }

    public class DoubleSearchAttribute : SearchAttribute {
        private readonly DoubleRelated _index;
        public DoubleSearchAttribute(DoubleRelated index) {
            _index = index;
        }
        public override int Index => (int)_index;
        public override Related Related => Related.DBL;
    }

    public class BoolSearchAttribute : SearchAttribute {
        private readonly BoolRelated _index;
        public BoolSearchAttribute(BoolRelated index) {
            _index = index;
        }
        public override int Index => (int)_index;
        public override Related Related => Related.BOOL;
    }

    public class GeoSearchAttribute : SearchAttribute {
        private readonly GeoRelated _index;
        public GeoSearchAttribute(GeoRelated index) {
            _index = index;
        }
        public override int Index => (int)_index;
        public override Related Related => Related.GEO;
    }

    public class EnumSearchAttribute : SearchAttribute {
        private readonly EnumRelated _index;
        public EnumSearchAttribute(EnumRelated index) {
            _index = index;
        }
        public override int Index => (int)_index;
        public override Related Related => Related.ENUM;
    }

    public class DateSearchAttribute : SearchAttribute {
        private readonly DateRelated _index;
        public DateSearchAttribute(DateRelated index) {
            _index = index;
        }
        public override int Index => (int)_index;
        public override Related Related => Related.DATE;
    }

    public enum Related { 
        REFERENCE = 0,
        LOCAL_REFERENCE = 1,
        STR = 2,
        SUGGESTION = 3,
        NUM64 = 4,
        NUM32 = 5,
        DBL = 6,
        BOOL = 7,
        GEO = 8,
        ENUM = 9,
        DATE = 10
    }

}