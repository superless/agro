namespace trifenix.connect.mdm.entity_model
{
    /// <summary>
    /// Enum, Fecha y entidades tienen facetable para agrupar resultados.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPropertyBaseFaceTable<T> : IProperty<T>, IPropertyFaceTable
    {
        
    }

    public class PropertyBaseFaceTable<T> : IProperty<T>, IPropertyFaceTable
    {
        public int index { get; set; }
        public T value { get; set; }

        public string facet { get; set; }
    }



}