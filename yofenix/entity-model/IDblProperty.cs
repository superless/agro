namespace trifenix.connect.mdm.entity_model
{
    /// <summary>
    /// una propiedad double
    /// </summary>
    public interface IDblProperty : IProperty<double> { }


    /// <summary>
    /// implementación para typegen.
    /// </summary>
    public class DblProperty : IProperty<double>
    {
        
        public int index { get; set; }
        public double value { get; set; }
    }



}