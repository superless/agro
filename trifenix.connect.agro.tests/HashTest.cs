using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using trifenix.connect.agro.external.hash;
using trifenix.connect.agro_model;
using trifenix.connect.search_mdl;
using trifenix.connect.util;
using Xunit;

namespace trifenix.connect.agro.tests
{
    public class HashTest
    {
        [Fact]
        public void InsertProductAndValidHash() {
            // arrange
            var hash = new HashEntityAgroSearch();

            
            var productHash = hash.HashHeader(typeof(Product));


            var productDict = new JsonDictionaryHeaders
            {
                index = 12,
                sug = new Dictionary<int, string> { { 1, "GENERIC_NAME" }, { 14, "SAG_CODE" } },
                str = new Dictionary<int, string> { { 13, "GENERIC_CORRELATIVE" } },
                rel = new Dictionary<int, string> { { 7, "INGREDIENT" }, { 32, "BRAND" } },
                enm = new Dictionary<int, string> { { 0, "GENERIC_MEASURE_TYPE" } },
                
            };

            var hashTestDict = Mdm.Reflection.Cripto.ComputeSha256Hash(JsonConvert.SerializeObject(productDict));


            Assert.True(productHash.Equals(hashTestDict));

        }

    }
}
