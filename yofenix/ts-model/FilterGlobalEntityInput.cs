namespace trifenix.connect.mdm.ts_model
{
    /// <summary>
    /// Filtro global que permitirá que las consultas incluyan un valor de entidad en sus consultas.
    /// </summary>
    public class FilterGlobalEntityInput {
        
        /// <summary>
        /// Indice de una entidad a filtrar
        /// </summary>
        public int indexMain { get; set; }



        //id de los indices de cada entidad seleccionada para filtro.
        public string[] EntitiesFounded { get; set; }

        /// <summary>
        /// Filtro global Recursivo, si existen propiedades que dependan 
        /// del filtro principal, por ejemplo si un filtro global fuera el año agricola,
        /// no todas las consultas tendran acceso a la entidad season directamente.
        /// por tanto puede inclir en este contenedor, todos los filtros que dependan del principal.
        /// </summary>
        public FilterGlobalEntityInput FilterChilds { get; set; }


        //clase para almacenar el resultado de los filtros globales.
        
    }

   

    


}
