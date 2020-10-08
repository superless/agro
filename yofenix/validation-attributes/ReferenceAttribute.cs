using System;

namespace trifenix.connect.mdm.validation_attributes
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