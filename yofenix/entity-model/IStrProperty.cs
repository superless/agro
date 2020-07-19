namespace trifenix.connect.mdm.entity_model
{
    /// <summary>
    /// propiedad string 
    /// </summary>
    public interface IStrProperty : IProperty<string>
    {

    }

    public class StrProperty : IProperty<string>
    {
        public int index { get; set; }
        public string value { get; set; }
    }



}