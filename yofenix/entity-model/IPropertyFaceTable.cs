namespace trifenix.connect.mdm.entity_model
{
    /// <summary>
    /// Enum, Fecha y entidades tienen facetable para agrupar resultados.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPropertyFaceTable<T> : IProperty<T>, IPropertyFaceTable
    {
       
    }

    /// <summary>
    /// Interfaz para entidades que implementen un facet de busqueda.
    /// </summary>
    public interface IPropertyFaceTable {

        /// <summary>
        /// facet de la entidad.
        /// el facet se genera desde el index y el id, quedando = index,id
        /// por ejemplo,
        /// si registramos una nueva  camara con marca canon, registrariamos la marca en esta entidad indicandole el índice que 
        /// corresponda a la marca en el diccionario, el id de la entidad (guid)
        /// y el facet que sería el index,guid
        /// registrariamos esta campo como
        /// {
        ///    index: 3,
        ///    id : "[guid]",
        ///    facet : "3,[guid]"
        /// }
        /// cuando hagamos consultas traerá las agrupaciones que encuentre en base al índice y el guid,
        /// permitiendo agrupar por entidad e identificador de esa entidad.
        /// por ejemplo,
        /// 3 => El índice que corresponde a la marca
        /// [guid] => un guid correspondería a una entidad de tipo marca, como Canon.
        /// </summary>
        string facet { get; }
    }



}