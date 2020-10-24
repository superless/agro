using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using trifenix.agro.external.operations.tests.data;
using trifenix.connect.agro.external;
using trifenix.connect.agro.external.hash;
using trifenix.connect.agro.external.helper;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.cosmos;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro.queries;
using trifenix.connect.agro.tests.data;
using trifenix.connect.agro_model;
using trifenix.connect.interfaces;
using trifenix.connect.interfaces.search;
using trifenix.connect.interfaces.upload;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.ts_model;
using trifenix.connect.tests.mock;
using trifenix.connect.util;

namespace trifenix.connect.agro.tests.mock
{

    /// <summary>
    /// Mocks de las llamadas a base de datos u otras conexiones.
    /// </summary>
    public static class MockHelper
    {


        /// <summary>
        /// AgroSearch usando mock de base de datos de busqueda
        /// </summary>
        /// <returns>AgroSearch</returns>
        public static IAgroSearch<GeoPointTs> AgroSearch() => new AgroSearch<GeoPointTs>(BaseSearch(), new SearchQueries(), new ImplementMock(), new HashEntityAgroSearch());

        /// <summary>
        /// AgroSearch usando mock de base de datos de busqueda
        /// </summary>
        /// <returns>AgroSearch</returns>
        public static IAgroSearch<GeoPointTs> AgroSearch(IBaseEntitySearch<GeoPointTs> baseSearch) => new AgroSearch<GeoPointTs>(baseSearch, new SearchQueries(), new ImplementMock(), new HashEntityAgroSearch());

        /// <summary>
        /// Retorna operaciones de base de datos de busqueda,
        /// esta clase es de trifenix connect.
        /// </summary>
        /// <returns></returns>
        public static IBaseEntitySearch<GeoPointTs> BaseSearchNewProductInput()
        {
            var mock = BaseSearchMock();
            mock.Setup(s => s.FilterElements(It.IsAny<string>())).Returns(new List<IEntitySearch<GeoPointTs>>());
            var prds = AgroData.Products.SelectMany(s=>Mdm.GetEntitySearch(new ImplementMock(), s, new HashEntityAgroSearch())).ToList();
            mock.Setup(s => s.FilterElements(It.IsAny<string>())).Returns((string query)=> prds.ToList());
            return mock.Object;
        }

        public static IBaseEntitySearch<GeoPointTs> BaseSearch() {

            var mock = BaseSearchMock();
            mock.Setup(s => s.FilterElements(It.IsAny<string>())).Returns(new List<IEntitySearch<GeoPointTs>>());
            return mock.Object;


        }

        public static Mock<IBaseEntitySearch<GeoPointTs>> BaseSearchMock() {
            var mock = new Mock<IBaseEntitySearch<GeoPointTs>>();
            mock.Setup(s => s.AddElement(It.IsAny<IEntitySearch<GeoPointTs>>()));
            mock.Setup(s => s.AddElements(It.IsAny<List<IEntitySearch<GeoPointTs>>>()));
            mock.Setup(s => s.DeleteElements(It.IsAny<List<IEntitySearch<GeoPointTs>>>()));
            mock.Setup(s => s.DeleteElements(It.IsAny<string>()));
            mock.Setup(s => s.EmptyIndex());
            
            mock.Setup(s => s.CreateOrUpdateIndex());
            return mock;
        }




        /// <summary>
        /// Retorna mock de correo
        /// </summary>
        /// <returns></returns>
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

            // Busqueda en colecciones de AgroInputData
            mockExistElement
                .Setup(s => s.ExistsById<Product>(It.IsAny<string>()))

                .ReturnsAsync((string id) => AgroInputData.Products.Any(s => s.Id.Equals(id)));

            mockExistElement
                .Setup(s => s.ExistsById<Ingredient>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {

                    return AgroInputData.Ingredients.Any(s => s.Id.Equals(id));
                });


            mockExistElement.Setup(s => s.ExistsById<Brand>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroInputData.Brands.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Variety>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroInputData.Varieties.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Specie>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroInputData.Species.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<CertifiedEntity>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroInputData.CertifiedEntities.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<ApplicationTarget>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroInputData.ApplicationTargets.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Season>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroInputData.Seasons.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Barrack>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroInputData.Barracks.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Sector>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroInputData.Sectors.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<PlotLand>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroInputData.PlotLands.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<Rootstock>(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return AgroInputData.Rootstocks.Any(s => s.Id.Equals(id));
                });

            mockExistElement.Setup(s => s.ExistsById<CostCenter>(It.IsAny<string>()))
               .ReturnsAsync((string id) =>
               {
                   return AgroInputData.CostCenters.Any(s => s.Id.Equals(id));
               });


            mockExistElement.Setup(s => s.ExistsById<BusinessName>(It.IsAny<string>()))
               .ReturnsAsync((string id) =>
               {
                   return AgroInputData.BusinessNames.Any(s => s.Id.Equals(id));
               });



            // busqueda en colecciones usando nombre propiedad y valor, y si existe un id, excluyendolo de la busqueda.
            mockExistElement
                .Setup(s => s.ExistsWithPropertyValue<Product>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>()))
                .ReturnsAsync((string nameProp, string propValue, string? id) => AgroInputData.Products.Any(s =>
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
                .ReturnsAsync((string nameProp, string propValue, string? id) => AgroInputData.Barracks.Any(s =>
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

        /// <summary>
        /// Mock de upload Image
        /// </summary>
        /// <returns></returns>
        public static IUploadImage UploadImage()
        {
            var uimage = new Mock<IUploadImage>();
            uimage.Setup(s => s.UploadImageBase64(It.IsAny<string>())).ReturnsAsync("image 01");
            return uimage.Object;
        }


        /// <summary>
        /// Mock de clima.
        /// </summary>
        /// <returns></returns>
        public static IWeatherApi WeatherApi()
        {
            var weatherApi = new Mock<IWeatherApi>();
            weatherApi.Setup(s => s.GetWeather(It.IsAny<float>(), It.IsAny<float>())).ReturnsAsync(new Weather { });
            return weatherApi.Object;
        }


        // Mock de conexiones a base de datos
        public static IDbAgroConnect Connect => new MockConnect();

    }
}
