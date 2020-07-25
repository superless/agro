using System;
using System.Linq;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.ts_model;
using trifenix.connect.test.enums;
using trifenix.connect.test.model;
using trifenix.connect.util;
using Xunit;


namespace trifenix.connect.test
{
    public class MdmTests
    {
        [Fact]
        public void GetDescriptionFromEnum()
        {
            //asign

            //action
            var dictionaryFromEnum = Mdm.Reflection.GetDescription(typeof(TEST1));
            // assert
            Assert.Equal("TEST1", dictionaryFromEnum.First().Value);

        }

        [Fact]
        public void ConvertObjectoToEntity() {
            // assign
            var objs = new BarrackTest()
            {
                ClientId = 1,
                GeographicalPoints = new GeoPointTs[] { new GeoPointTs { latitude = 1.3, longitude = 1.45 } },
                Hectares = 3.4,
                Id = Guid.NewGuid().ToString("N"),
                IdPlotLand = Guid.NewGuid().ToString("N"),
                IdPollinator = Guid.NewGuid().ToString("N"),
                IdRootstock = Guid.NewGuid().ToString("N"),
                IdVariety = Guid.NewGuid().ToString("N"),
                Name = "Barrack1",
                NumberOfPlants = 1221,
                PlantingYear = 1982,
                SeasonId = Guid.NewGuid().ToString("N")
            };

            // action
            var entities = Mdm.GetEntitySearch(new Implementation(), objs, typeof(EntityBaseSearch));

            Assert.Equal(entities.First().str.First().value, objs.Name);


        }
    }
}
