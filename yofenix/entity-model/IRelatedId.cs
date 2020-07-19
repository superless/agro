namespace trifenix.connect.mdm.entity_model
{
    /// <summary>
    /// Interface que representa una entidad dentro de un entitySearch
    /// </summary>
    public interface IRelatedId : IPropertyFaceTable
    {
        /// <summary>
        /// índice de la entidad
        /// </summary>
        int index { get; set; }

        /// <summary>
        /// identificador de la entidad
        /// </summary>
        string id { get; set; }

    }

    public class RelatedId : IPropertyFaceTable {


        public string facet {get; set;}

        public int index { get; set; }
      
        public string id { get; set; }



    }



}