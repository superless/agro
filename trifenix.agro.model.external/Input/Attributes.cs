using System;

namespace trifenix.agro.model.external.Input {

    //Solo asignar a valores primitivos
    public class Unique : Attribute { }

    //Solo asignar a Id(String)
    public class Reference : Attribute {

        public Type entityOfReference;
        public Reference(Type _entityOfReference) {
            entityOfReference = _entityOfReference;
        }

    }

    //Asignar a cualquier tipo de dato
    //public class Required : Attribute { }

}