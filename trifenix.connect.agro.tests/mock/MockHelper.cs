using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using trifenix.agro.external.operations.tests.data;
using trifenix.connect.agro.external;
using trifenix.connect.agro.external.helper;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.cosmos;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro.queries;
using trifenix.connect.agro_model;
using trifenix.connect.interfaces;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.interfaces.search;
using trifenix.connect.interfaces.upload;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.ts_model;

namespace trifenix.connect.agro.tests.mock
{

    /// <summary>
    /// Mocks de las llamadas a base de datos u otras conexiones.
    /// </summary>
    public static class MockHelper
    {

        public static IAgroSearch<GeoPointTs> AgroSearch() => new AgroSearch<GeoPointTs>(new SearchQueryOperations<GeoPointTs>(BaseSearch(), new SearchQueries()), BaseSearch(), new EntitySearchMgmt<GeoPointTs>(BaseSearch(), new ImplementMock()));


        public static IBaseEntitySearch<GeoPointTs> BaseSearch()
        {

            var mock = new Mock<IBaseEntitySearch<GeoPointTs>>();
            mock.Setup(s => s.AddElement(It.IsAny<IEntitySearch<GeoPointTs>>()));
            mock.Setup(s => s.AddElements(It.IsAny<List<IEntitySearch<GeoPointTs>>>()));
            mock.Setup(s => s.DeleteElements(It.IsAny<List<IEntitySearch<GeoPointTs>>>()));
            mock.Setup(s => s.DeleteElements(It.IsAny<string>()));
            mock.Setup(s => s.EmptyIndex());
            mock.Setup(s => s.FilterElements(It.IsAny<string>())).Returns(new List<IEntitySearch<GeoPointTs>>());
            mock.Setup(s => s.CreateOrUpdateIndex());
            return mock.Object;
        }


        public static IEmail Email()
        {
            var mail = new Mock<IEmail>();

            mail.Setup(s => s.SendEmail(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()));

            return mail.Object;
        }

        /// <summary>
        /// Mock de las consultas de existencia de elementos.
        /// </summary>
        /// <returns></returns>
        public static IDbExistsElements GetExistElement()
        {

            var mockExistElement = new Mock<IDbExistsElements>();

            // Busqueda en colecciones de agrodata
            mockExistElement
                .Setup(s => s.ExistsById<Product>(It.IsAny<string>()))

                .ReturnsAsync((string id) => AgroData.Products.Any(s => s.Id.Equals(id)));

            mockExistElement
                .Setup(s => s.ExistsById<Ingredient>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {

                    return AgroData.Ingredients.Any(s => s.Id.Equals(id));
                });


            mockExistElement.Setup(s => s.ExistsById<Brand>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.Brands.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Variety>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.Varieties.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Specie>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.Species.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<CertifiedEntity>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.CertifiedEntities.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<ApplicationTarget>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.ApplicationTargets.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Season>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.Seasons.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Barrack>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.Barracks.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Sector>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.Sectors.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<PlotLand>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.PlotLands.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Rootstock>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroData.Rootstocks.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<CostCenter>(It.IsAny<string>()))
               .ReturnsAsync((string id) =>
               {
                   return AgroData.CostCenters.Any(s => s.Id.Equals(id));
               });


            mockExistElement.Setup(s => s.ExistsById<BusinessName>(It.IsAny<string>()))
               .ReturnsAsync((string id) =>
               {
                   return AgroData.BusinessNames.Any(s => s.Id.Equals(id));
               });



            // busqueda en colecciones usando nombre propiedad y valor, y si existe un id, excluyendolo de la busqueda.
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

            mockExistElement
                .Setup(s => s.ExistsWithPropertyValue<Barrack>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>()))
                .ReturnsAsync((string nameProp, string propValue, string? id) => AgroData.Barracks.Any(s =>
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

        public static IUploadImage UploadImage()
        {
            var uimage = new Mock<IUploadImage>();
            uimage.Setup(s => s.UploadImageBase64(It.IsAny<string>())).ReturnsAsync("image 01");
            return uimage.Object;
        }

        public static IWeatherApi WeatherApi()
        {
            var weatherApi = new Mock<IWeatherApi>();
            weatherApi.Setup(s => s.GetWeather(It.IsAny<float>(), It.IsAny<float>())).ReturnsAsync(new Weather { });
            return weatherApi.Object;
        }

        public static IDbAgroConnect Connect => new MockConnect();

    }
}
