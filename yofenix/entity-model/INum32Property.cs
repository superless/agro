namespace trifenix.connect.mdm.entity_model
{

    /// <summary>
    /// una propiedad de tipo entero
    /// </summary>
    public interface INum32Property : IProperty<int>
    {

    }

    public class Num32Property : INum32Property
    {
        public int index { get; set; }
        public int value { get; set; }
    }



}