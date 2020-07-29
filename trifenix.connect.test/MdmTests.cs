using System;
using System.Linq;
using trifenix.connect.mdm.entity_model;
using trifenix.connect.mdm.ts_model;
using trifenix.connect.search_mdl;
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
            var entities = Mdm.GetEntitySearch(new Implementation(), objs, typeof(EntityBaseSearch<GeoPointTs>));

            // assert
            // test simple para verificar que es el mismo nombre.
            Assert.Equal(entities.First().str.First().value, objs.Name);
        }

        [Fact]
        public void ConvertEntityToObject() {

            // assign
            var entity = new EntityBaseSearch<GeoPointTs>
            {
                bl = Array.Empty<BoolBaseProperty>(),
                created = DateTime.Now,
                index = (int)EntityRelated.BARRACK,
                dbl = new DblBaseProperty[] { new DblBaseProperty { index = (int)DoubleRelated.HECTARES, value = 3.4 } },
                dt = Array.Empty<DtBaseProperty>(),
                enm = Array.Empty<EnumBaseProperty>(),
                geo = new GeographyProperty[] { new GeographyProperty { index = (int)GeoRelated.LOCATION_BARRACK, value = new GeoPointTs { latitude = 1.3, longitude = 1.45 } } },
                id = Guid.NewGuid().ToString("N"),
                num32 = new Num32BaseProperty[] { new Num32BaseProperty { index = (int)NumRelated.NUMBER_OF_PLANTS, value = 1221 } },
                num64 = new Num64BaseProperty[] { new Num64BaseProperty { index = (int)NumRelated.GENERIC_CORRELATIVE, value = 1 } },
                rel = new IRelatedId[] {
                    new RelatedBaseId{ id = Guid.NewGuid().ToString("N"), index = (int)EntityRelated.PLOTLAND },
                    new RelatedBaseId{ id = Guid.NewGuid().ToString("N"), index = (int)EntityRelated.POLLINATOR },
                    new RelatedBaseId{ id = Guid.NewGuid().ToString("N"), index = (int)EntityRelated.ROOTSTOCK },
                    new RelatedBaseId { id = Guid.NewGuid().ToString("N"), index = (int)EntityRelated.VARIETY },
                    new RelatedBaseId { id = Guid.NewGuid().ToString("N"), index = (int)EntityRelated.SEASON },
                },
                str = new StrBaseProperty[] { new StrBaseProperty { index = (int)StringRelated.GENERIC_NAME, value = "BarrackEntity" } },
                sug = Array.Empty<StrBaseProperty>()

            };

            // action 
            var barrack = (BarrackTest)Mdm.GetEntityFromSearch(entity, typeof(BarrackTest), "trifenix.connect.test.model", s => s, new SearchElement());


            //assert
            Assert.Equal(entity.str.First().value, barrack.Name);


        }
    }

    public class SearchElement : ISearchEntity<GeoPointTs>
    {
        public IEntitySearch<GeoPointTs> GetEntity(int entityKind, string idEntity)
        {
            return new EntityBaseSearch<GeoPointTs>();
        }
    }
}
