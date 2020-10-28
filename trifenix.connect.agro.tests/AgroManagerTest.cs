using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.external;
using trifenix.agro.external.operations.tests.data;
using trifenix.connect.agro.tests.data;
using trifenix.connect.agro.tests.mock;
using trifenix.connect.agro_model;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.ts_model;
using Xunit;

namespace trifenix.connect.agro.tests
{
    public class AgroManagerTest
    {


        /// <summary>
        /// Ingresamos un producto nuevo y validamos que se haya guardado en la base de datos.
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task InsertProductAndCheckIfSaveOkInDb() {
            //assign
            var agroManager = new AgroManager<GeoPointTs>(MockHelper.Connect(), MockHelper.Email(), MockHelper.UploadImage(), MockHelper.WeatherApi(), MockHelper.AgroSearch(MockHelper.BaseSearch()), string.Empty);

            var prd = AgroInputData.Product1;
            prd.SagCode = "0123A";
            prd.Name = "Nuevo Producto a insertar";
            prd.Id = string.Empty;
            prd.Doses[0].IdProduct = string.Empty;
            prd.Doses[1].IdProduct = string.Empty;
            
            //action
            var productResult = await agroManager.Product.SaveInput(prd);


            Assert.Contains(AgroData.Products, s => !new Product[] { AgroData.Product1, AgroData.Product2 }.Any(t => t.Id.Equals(s.Id)));
        

            //assert


        }
    }
}
