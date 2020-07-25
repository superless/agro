using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.mdm.ts_model.props
{
    public class Num64Property : IProperty<long>
    {
        public int index { get; set; }
        public long value { get; set; }
    }
}
