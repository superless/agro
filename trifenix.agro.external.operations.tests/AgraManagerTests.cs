using Cosmonaut;
using Moq;
using System;
using trifenix.agro.db;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.interfaces;
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.ts_model;
using Xunit;
using trifenix.agro.external.operations;
using trifenix.agro.email.interfaces;
using trifenix.agro.weather.interfaces;
using trifenix.agro.search.interfaces;
using trifenix.agro.storage.interfaces;
using trifenix.connect.agro_model_input;

namespace trifenix.agro.external.operations.tests
{
    public class AgraManagerTests
    {

        private IAgroManager<GeoPointTs> agroManager;

        public AgraManagerTests()
        {
            var mockEmail = new Mock<IEmail>();

            var mockWeather = new Mock<IWeatherApi>();

            var mockUploadImage = new Mock<IUploadImage>();
            var mockSearch = new Mock<IAgroSearch<GeoPointTs>>();

            agroManager = new AgroManager<GeoPointTs>(new MockConnect(), mockEmail.Object, mockUploadImage.Object, mockWeather.Object, mockSearch.Object, string.Empty, false);

        }


        [Fact]
        public void CreandoProducto()
        {
            // assign
            var productInput = new ProductInput
            {
                Id = "1",
                IdActiveIngredient = "2",
                IdBrand = "3",
                MeasureType = connect.agro.index_model.enums.MeasureType.KL, // quitar
                SagCode = "11223",
                Name = "Producto1",
                Doses = new DosesInput[2] { new DosesInput { }, new DosesInput { } }

            };


            //action


            //assert
        }


    }

    public class MockConnect : IDbConnect
    {

        // no se usa en la mayoría de las operaciones
        public ICosmosStore<EntityContainer> BatchStore { 
            get {

                return null;
            }    
        }


        public ICommonQueries CommonQueries {  
            get {
                var mock = new Mock<ICommonQueries>();
                // definición de métodos.


                
                return mock.Object;
            }
        }

        public IGraphApi GraphApi
        {
            get
            {
                var mock = new Mock<IGraphApi>();
                // definición de métodos.



                return mock.Object;
            }
        }

        public IExistElement ExistsElements(bool isBatch)
        {
            var mock = new Mock<IExistElement>();
            // definición de métodos.
            return mock.Object;
        }


        public ICommonDbOperations<T> GetCommonDbOp<T>() where T : DocumentBase
        {
            var mock = new Mock<ICommonDbOperations<T>>();
            // definición de métodos.


            return mock.Object;
        }

        public IMainGenericDb<T> GetMainDb<T>() where T : DocumentBase
        {
            var mock = new Mock<IMainGenericDb<T>>();
            // definición de métodos.


            return mock.Object;
        }
    }
}
