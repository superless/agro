namespace trifenix.connect.mdm.entity_model
{
    /// <summary>
    /// Una propiedad booleana
    /// </summary>
    public interface IBoolProperty : IPropertyBaseFaceTable<bool>{}


    public class BoolProperty : IPropertyBaseFaceTable<bool>
    {
        public int index { get; set; }
        public bool value { get; set; }

        public string facet { get; set; }
    }


}