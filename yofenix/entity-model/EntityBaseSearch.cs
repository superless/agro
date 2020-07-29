using System;
using trifenix.connect.mdm.ts_model;

namespace trifenix.connect.mdm.entity_model
{
    public class EntityBaseSearch<T> : IEntitySearch<T>
    {
        public string id { get; set; }
        public DateTime created { get; set; }
        public int index { get; set; }

        public IBoolProperty[] bl { get; set; }
        public IDblProperty[] dbl { get; set; }
        public IDtProperty[] dt { get; set; }
        public IEnumProperty[] enm { get; set; }
        public INum32Property[] num32 { get; set; }
        public INum64Property[] num64 { get; set; }
        public IRelatedId[] rel { get; set; }
        public IStrProperty[] str { get; set; }
        public IStrProperty[] sug { get; set; }
        public IProperty<T>[] geo { get; set; }
    }



}