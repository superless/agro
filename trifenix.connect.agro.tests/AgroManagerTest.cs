using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.external;
using trifenix.agro.external.operations.tests.data;
using trifenix.connect.agro.tests.mock;
using trifenix.connect.mdm.ts_model;
using Xunit;

namespace trifenix.connect.agro.tests
{
    public class AgroManagerTest
    {
        [Fact]
        public async Task InsertProduct() {
            //assign
            var agroManager = new AgroManager<GeoPointTs>(MockHelper.Connect, MockHelper.Email(), MockHelper.UploadImage(), MockHelper.WeatherApi(), MockHelper.AgroSearch(MockHelper.BaseSearchNewProductInput()), string.Empty);
            //action
            var productResult = await agroManager.Product.SaveInput(AgroInputData.Product1);

            //assert


        }
    }
}
