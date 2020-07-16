using trifenix.connect.mdm.enums;

namespace trifenix.connect.mdm.ts_model
{
    /// <summary>
    /// Item de Ordernamiento, permite ordenar una consulta.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Ascendente o descendente.
        /// </summary>
        public bool Desc { get; set; }


        /// <summary>
        /// Tipo de propiedad
        /// </summary>
        public KindProperty KindProperty { get; set; }

        /// <summary>
        /// Indice de la propiedad
        /// </summary>
        public int PropertyIndex { get; set; }
    }

   

    


}
