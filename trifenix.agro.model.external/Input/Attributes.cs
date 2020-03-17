using System;

namespace trifenix.agro.model.external.Input {
    public class Unique : Attribute { }
    public class Reference : Attribute {

        public Type entityOfReference;
        public Reference(Type _entityOfReference) {
            entityOfReference = _entityOfReference;
        }

    }

}