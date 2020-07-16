

using trifenix.connect.ts_model.enums;

namespace trifenix.connect.mdm.ts_model
{
    /// <summary>
    /// Filtro para cada tipo de valor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FilterBase<T> {        

        /// <summary>
        /// Tipo de filtro
        /// </summary>
        public FilterType FilterType { get; set; }

        /// <summary>
        /// valor a filtrar
        /// </summary>
        public T Value { get; set; }


    }

   

    


}
