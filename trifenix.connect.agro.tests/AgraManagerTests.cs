using Cosmonaut;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.external;
using trifenix.agro.external.operations.tests.data;
using trifenix.connect.agro.external.helper;
using trifenix.connect.agro.interfaces;
using trifenix.connect.agro.interfaces.cosmos;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.agro.tests.mock;
using trifenix.connect.entities.cosmos;
using trifenix.connect.input;
using trifenix.connect.interfaces;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.interfaces.external;
using trifenix.connect.interfaces.graph;
using trifenix.connect.interfaces.upload;
using trifenix.connect.mdm.ts_model;
using Xunit;

namespace trifenix.connect.agro.tests
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

            agroManager = new AgroManager<GeoPointTs>(MockHelper.Connect, mockEmail.Object, mockUploadImage.Object, mockWeather.Object, mockSearch.Object, string.Empty, false);

        }


        [Theory]
        [InlineData("9aebaf15-eb85-49d7-acca-643329d4078b")]
        [InlineData("7990893f-74e1-45d6-8f3d-af1c9896842c")]
        public async Task CreandoProductoSinDosis(string id) 
        {
            // assign
            var productInput = AgroData.Products.First(p => p.Id.Equals(id));
            // eliminando id para crear un producto (si tiene id es editar).
            

            //action
            var result =   await agroManager.Product.SaveInput(productInput, false);



            //assert

        }


    }

    
}
