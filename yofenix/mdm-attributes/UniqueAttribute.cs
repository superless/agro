using System;

namespace trifenix.connect.mdm_attributes
{
    //Solo asignar a valores primitivos
    /// <summary>
    /// Determina si un campo debe ser único, esto permitirá
    /// validarlo en el servidor y entregar metadata al cliente
    /// para que también lo valide.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueAttribute : Attribute { }



}
