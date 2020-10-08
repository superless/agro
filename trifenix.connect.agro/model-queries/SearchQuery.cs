namespace trifenix.connect.agro.model_queries
{
    /// <summary>
    /// enumeración para diccionario de consultas en search.
    /// </summary>
    public enum SearchQuery {
        // obtiene entidades con un indice como filtro y que tenga entidades relacionadas que tengan un índice y un identificador como filtro
        ENTITIES_WITH_ENTITYID = 0,
        // sirve para comprobar si un elemento existe, sin consultar por si mismo, por ejemplo, quiero saber si existe mi nombre en la base, pero no me incluya a mi.
        ENTITIES_WITH_ENTITYID_EXCEPTID = 1,

        // Consulta para obtener un elemento
        GET_ELEMENT = 2
    }
}
