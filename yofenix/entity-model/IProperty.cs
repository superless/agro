namespace trifenix.connect.mdm.entity_model
{
    /// <summary>
    /// Interface base para todas las propiedades en el que se especifica el índice y el valor.
    /// </summary>
    /// <typeparam name="T">Tipo de Valor (bool, string, int, long, double, datetime)</typeparam>
    public interface IProperty<T> {

        /// <summary>
        /// Índice de una propiedad
        /// </summary>
        int index { get; set; }

        /// <summary>
        /// Valor de una entidad
        /// </summary>
        T value { get; set; }
    }



}