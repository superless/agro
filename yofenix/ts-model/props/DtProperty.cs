using System;
using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.mdm.ts_model.props
{
    public class DtProperty : IPropertyBaseFaceTable<DateTime>
    {
        public int index { get; set; }
        public DateTime value { get; set; }

        public string facet { get; set; }

    }
}
