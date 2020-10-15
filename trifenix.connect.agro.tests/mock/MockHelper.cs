using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using trifenix.agro.external.operations.tests.data;
using trifenix.connect.agro_model;
using trifenix.connect.interfaces.db.cosmos;

namespace trifenix.connect.agro.tests.mock
{
    public static class MockHelper
    {
        public static IExistElement GetExistElement()
        {

            var mockExistElement = new Mock<IExistElement>();

            mockExistElement
                .Setup(s => s.ExistsById<Product>(It.IsAny<string>()))
                .ReturnsAsync((string id) => AgroData.Products.Any(s => s.Id.Equals(id)));

            mockExistElement
                .Setup(s => s.ExistsById<Ingredient>(It.IsAny<string>()))
                .ReturnsAsync((string id) => {

                    return AgroData.Ingredients.Any(s => s.Id.Equals(id));
                });


            mockExistElement.Setup(s => s.ExistsById<Brand>(It.IsAny<string>()))
                .ReturnsAsync((string id) => {
                    return AgroData.Brands.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Variety>(It.IsAny<string>()))
                .ReturnsAsync((string id) => {
                    return AgroData.Varieties.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Specie>(It.IsAny<string>()))
                .ReturnsAsync((string id) => {
                    return AgroData.Species.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<CertifiedEntity>(It.IsAny<string>()))
                .ReturnsAsync((string id) => {
                    return AgroData.CertifiedEntities.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<ApplicationTarget>(It.IsAny<string>()))
                .ReturnsAsync((string id) => {
                    return AgroData.ApplicationTargets.Any(s => s.Id.Equals(id));
                });



            mockExistElement
                .Setup(s => s.ExistsWithPropertyValue<Product>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>()))
                .ReturnsAsync((string nameProp, string propValue, string? id) => AgroData.Products.Any(s =>
                {

                    var value = s.GetType().GetProperty(nameProp).GetValue(s);

                    var rslt = value.Equals(propValue)
                    &&
                    (string.IsNullOrWhiteSpace(id) || !s.Id.Equals(id));

                    return rslt;


                }
                ));


            return mockExistElement.Object;

        }
    }
}
