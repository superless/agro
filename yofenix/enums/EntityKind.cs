namespace trifenix.connect.mdm.enums
{

    /// <summary>
    /// Determina el tipo de entidad,
    /// esto permitirá saber que entidades siguen un patrón común por 
    /// tanto son mayormente susceptibles para ser automatizadas, como es el caso
    /// de Entity
    /// </summary>
    public enum EntityKind
    {
        // Tipo Entity, significa que es un mantenedor común.
        ENTITY = 0,

        // no debe ser generado
        CUSTOM_ENTITY = 1,

        // si generación es más compleja, por tanto se debe evaluar.
        PROCESS = 2
    }




}
