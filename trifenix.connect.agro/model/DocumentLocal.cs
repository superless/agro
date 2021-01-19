using System;
using System.Collections.Generic;
using System.Text;



namespace trifenix.connect.agro_model
{

#if DEBUG
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
