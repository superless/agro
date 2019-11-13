using Moq;
using System;
using trifenix.agro.common.tests.fakes;
using trifenix.agro.db.interfaces.agro.ext;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.operations.entities.ext;
using trifenix.agro.external.operations.tests.helper.Moqs;

namespace trifenix.agro.external.operations.tests.helper.Instances
{

    public static class ProductInstances
    {
        

        public static Mock<IProductRepository> GetInstance(Results result) =>
            MoqGenerator.GetMoqResult<IProductRepository, Product>(
                result, 
                (s) => s.CreateUpdateProduct(It.IsAny<Product>()), 
                (s) => s.GetProduct(It.IsAny<string>()), 
                s => s.GetProducts());


        public static ProductOperations GetProductOperations(ProductEnumIntances instance) {

            switch (instance)
            {
                case ProductEnumIntances.DefaultInstance:
                    return new ProductOperations(
                        IngredientsInstances.GetInstance(Results.Values).Object,
                        GetInstance(Results.Values).Object,
                        ApplicationTargetInstances.GetInstance(Results.Values).Object,
                        CertifiedEntitiesInstances.GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        SpeciesIntances.GetInstance(Results.Values).Object,
                        CommonDbInstances<Product>.GetInstance(Results.Nullables).Object,
                        FakeGenerator.CreateString()
                        );
                case ProductEnumIntances.InstanceNoIngredientOnDb:
                    return new ProductOperations(
                        IngredientsInstances.GetInstance(Results.Empty).Object,
                        GetInstance(Results.Values).Object,
                        ApplicationTargetInstances.GetInstance(Results.Values).Object,
                        CertifiedEntitiesInstances.GetInstance(Results.Values).Object,
                        VarietyInstances.GetInstance(Results.Values).Object,
                        SpeciesIntances.GetInstance(Results.Values).Object,
                        CommonDbInstances<Product>.GetInstance(Results.Values).Object,
                        FakeGenerator.CreateString()
                        );
                case ProductEnumIntances.DefaultInstanceNullIds:
                    return new ProductOperations(
                        IngredientsInstances.GetInstance(Results.Values).Object,
                        GetInstance(Results.Values).Object,
                        ApplicationTargetInstances.GetInstance(Results.Nullables).Object,
                        CertifiedEntitiesInstances.GetInstance(Results.Nullables).Object,
                        VarietyInstances.GetInstance(Results.Nullables).Object,
                        SpeciesIntances.GetInstance(Results.Nullables).Object,
                        CommonDbInstances<Product>.GetInstance(Results.Nullables).Object,
                        FakeGenerator.CreateString()
                        );
            }
            throw new Exception("bar params");

        }
    }
}
