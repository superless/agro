namespace trifenix.connect.mdm.enums
{
    /// <summary>
    /// Determina si una entidad es local, es decir no tiene key value en una base de datos documental
    /// o es de referencia, por lo tanto, si tiene un key value en una base documental.
    /// </summary>
    public enum KindEntityProperty {
        REFERENCE = 0,
        LOCAL_REFERENCE = 1,
    }
}



