namespace trifenix.connect.mdm.ts_model
{
    /// <summary>
    /// Filtro global seleccionado, contenedor de filtro global, en una aplicación antes de hacer consultas se preguntará por los filtros globales.
    /// un filtro global tendrá un índice principal que será el tipo de entidad que será filtrada. 
    /// por tanto, en el cliente debería existir una selección, que consultará por todos los elementos del tipo de entidad.
    /// los resultados seleccionados serán guardados en EntitiesSelected.
    /// Se pueden definir también filtros que dependan del filtro padre.
    /// esto significa que el resultado seleccionado, servirá para filtrar los filtros hijos.
    /// por ejemplo:
    /// en el caso de un programa agricola existirá el filtro de centro de costos, el que determinará la empresa con la que operará el programa
    /// no todas las entidades tendrán directamente el centro de costos como relación, existirán algunas que dependerán de otra entidad para hacer la conexión.
    /// un filtro global debe considerar el centro de costos y todas las entidades que permitan filtrar todos los elementos del sitio a partir de esta selección.
    /// </summary>
    public class FilterGlobalEntityInput {
        
        /// <summary>
        /// Indice de una entidad a filtrar
        /// </summary>
        public int indexMain { get; set; }



        /// <summary>
        /// Identificador de los elementos seleccionados si es el padre y los ids que dependen del filtro principal para el hijo.
        /// </summary>
        public string[] EntitiesSelected { get; set; }

        /// <summary>
        /// Filtro global Recursivo, si existen propiedades que dependan 
        /// del filtro principal, por ejemplo si un filtro global fuera el año agricola,
        /// no todas las consultas tendran acceso a la entidad season directamente.
        /// por tanto puede incluir en este contenedor, todos los filtros que dependan del principal.
        /// </summary>
        public FilterGlobalEntityInput[] FilterChilds { get; set; }




        


    }

   

    


}
