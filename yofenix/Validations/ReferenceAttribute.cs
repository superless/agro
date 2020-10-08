using System;

namespace trifenix.connect.mdm.Validations
{

    

    //Solo asignar a Id(String)
    public class ReferenceAttribute : Attribute {

        public Type entityOfReference;
        public ReferenceAttribute(Type _entityOfReference) {
            entityOfReference = _entityOfReference;
        }

    }

    //Asignar a cualquier tipo de dato
    //public class Required : Attribute { }

}