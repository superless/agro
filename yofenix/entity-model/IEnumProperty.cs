namespace trifenix.connect.mdm.entity_model
{
    /// <summary>
    /// una propiedad de tipo enumeración.
    /// </summary>
    public interface IEnumProperty : IPropertyBaseFaceTable<int>
    {

    }


    public class EnumProperty : IPropertyBaseFaceTable<int>
    {
        public int index { get; set; }

        public int value { get; set; }

        public string facet { get; set; }
    }



}