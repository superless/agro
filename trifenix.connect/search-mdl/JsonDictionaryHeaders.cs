using System;
using System.Collections.Generic;

namespace trifenix.connect.search_mdl
{
    public class JsonPreDictionaryHeaders
    {

        public int index { get; set; }

        public int[] rel { get; set; }
        public int[] str { get; set; }
        public int[] sug { get; set; }
        public int[] enm { get; set; }
        public int[] num32 { get; set; }
        public int[] num64 { get; set; }
        public int[] dbl { get; set; }
        public int[] dt { get; set; }
        public int[] bl { get; set; }
        public int[] geo { get; set; }


    }

    public class JsonDictionaryHeaders
    {

        public int index { get; set; }

        public IDictionary<int, string> rel { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> str { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> sug { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> enm { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> num32 { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> num64 { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> dbl { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> dt { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> bl { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> geo { get; set; } = new Dictionary<int, string>();


    }
}
