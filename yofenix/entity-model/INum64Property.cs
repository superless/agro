namespace trifenix.connect.mdm.entity_model
{
    /// <summary>
    /// una propiedad de tipo long
    /// </summary>
    public interface INum64Property : IProperty<long>
    {
    }

    public class Num64Property : IProperty<long> {
        public int index { get; set; }
        public long value { get; set; }
    }
}