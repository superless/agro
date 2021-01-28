using System;
using System.Collections.Generic;
using System.Text;


/// <summary>
/// Función encargada de condicionar la forma de compilar e ingresar paramentros de BD
/// </summary>
namespace trifenix.connect.agro_model
{

#if CONNECT
     using trifenix.connect.model;
    public abstract class DocumentLocal : DocumentDb
    {
       
    }
#else
    using Cosmonaut;
    using trifenix.connect.entities.cosmos;
    public abstract class DocumentLocal : DocumentBase, ISharedCosmosEntity
    {
       
    }

#endif
}
