namespace trifenix.connect.mdm.ts_model
{
    /// <summary>
    /// Facet, o agrupación de busqueda, es un groupby por categoria inserto en una busqueda.
    /// </summary>
    public class Facet {

        /// <summary>
        /// índice de la entidad del facet.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Título del facet, este valor se va a buscar con el valor. 
        /// el titulo se determina con el indice de la propiedad que representaria el nombre de la entidad
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// valor del facet, normalmente el identificador de una entidad.
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        /// Número de elementos encontrados, por cada tipo de count.
        /// </summary>
        public long Count { get; set; }
    }

   

    


}
