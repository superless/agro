using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.mdm.ts_model.props
{
    public class EnumProperty : IPropertyBaseFaceTable<int>
    {
        public int index { get; set; }

        public int value { get; set; }

        public string facet { get; set; }
    }
}
