using System;

namespace trifenix.connect.mdm.entity_model
{
    /// <summary>
    /// una propiedad de tipo fecha
    /// </summary>
    public interface IDtProperty : IPropertyBaseFaceTable<DateTime> { }



    public class DtProperty : IDtProperty
    {
        public int index { get; set; }
        public DateTime value { get; set; }

        public string facet { get; set; }

    }

}