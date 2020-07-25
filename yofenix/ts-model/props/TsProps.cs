using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.mdm.ts_model.props
{
    class TsProps
    {
    }

    public class StrProperty : IProperty<string>
    {
        public int index { get; set; }
        public string value { get; set; }
    }
}
