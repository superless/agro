using trifenix.connect.mdm.entity_model;

namespace trifenix.connect.mdm.ts_model
{
    /// <summary>
    /// Clase que hereda del relatedId (entidad dentro de un entitySearch),
    /// y le agrega un nombre
    /// </summary>
    public class ReletadIdTs : IRelatedId {

        /// <summary>
        /// Nombre de la entidad o coleccion de nombres.
        /// </summary>
        public string[] Name { get; set; }
        public int index { get; set; }
        public string id { get; set; }

        public string facet { get; set; }
    }

   

    


}
