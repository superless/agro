using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.mdm.ts_model.props
{
    public class Num32Property : IProperty<int>
    {
        public int index { get; set; }
        public int value { get; set; }
    }
}
