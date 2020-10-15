using Moq;
using Xunit;
using trifenix.agro.external.operations.tests.data;
using System.Linq;
using trifenix.connect.mdm.ts_model;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.interfaces.external;
using Cosmonaut;
using trifenix.connect.entities.cosmos;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.graph;
using trifenix.connect.input;
using trifenix.connect.interfaces;
using trifenix.connect.agro.interfaces;
using trifenix.connect.interfaces.upload;
using trifenix.connect.agro.interfaces.cosmos;

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


        [Theory]
        [InlineData("9aebaf15-eb85-49d7-acca-643329d4078b")]
        [InlineData("7990893f-74e1-45d6-8f3d-af1c9896842c")]
        public void CreandoProductoSinDosis(string id) 
        {
            // assign
            var productInput = AgroData.Products.First(p => p.Id.Equals(id));
            // eliminando id para crear un producto (si tiene id es editar).
            productInput.Id = string.Empty;






            //action
            agroManager.Product.SaveInput(productInput, false);



            //assert

        }


    }



    public class MockConnect : IDbAgroConnect
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

        public IDbExistsElements GetDbExistsElements => throw new System.NotImplementedException();

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

        public IValidatorAttributes<T_INPUT, T_DB> GetValidator<T_INPUT, T_DB>(bool isBatch)
            where T_INPUT : InputBase
            where T_DB : DocumentBase
        {
            var mock = new Mock<IValidatorAttributes<T_INPUT, T_DB>>();
            // definición de métodos.


            return mock.Object;
        }
    }
}
