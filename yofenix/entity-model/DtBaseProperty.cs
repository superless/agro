using System;

namespace trifenix.connect.mdm.entity_model
{
    public class DtBaseProperty : IDtProperty
    {
        public int index { get; set; }
        public DateTime value { get; set; }

        public string facet { get; set; }

    }

}