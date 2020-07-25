using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.mdm.ts_model.props
{
    public class BoolProperty : IPropertyBaseFaceTable<bool>
    {
        public int index { get; set; }
        public bool value { get; set; }

        public string facet { get; set; }
    }
}
