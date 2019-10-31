using System;
using trifenix.agro.common.tests.interfaces;
using trifenix.agro.external.operations.entities.ext;

namespace trifenix.agro.external.operations.tests.helper.Instances
{
    public static class ProductInstances
    {
        public static ProductOperations GetProductOperations(ProductEnumIntances instance) {

            switch (instance)
            {
                case ProductEnumIntances.DefaultInstance:
                    return new ProductOperations(AgroMoq.Ingredient.GetMoqRepo(Results.Values).Object);
             
            }
            throw new Exception("bar params");

        }
    }
}

public enum ProductEnumIntances {
    DefaultInstance
}
