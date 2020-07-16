namespace trifenix.connect.mdm.enums
{

    /// <summary>
    /// Tipo de resultado al momento de insertar un elemento.
    /// </summary>
    public enum ExtMessageResult {
        Ok,
        Error,
        Warning,
        ElementAlreadyExists,
        ChildRequiredDoesNotExists,
        ElementToEditDoesNotExists
    }
}
