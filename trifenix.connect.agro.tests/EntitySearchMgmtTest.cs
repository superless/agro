using AutoMapper;
using System.Linq;
using System.Text.RegularExpressions;
using trifenix.agro.external.operations.tests.data;
using trifenix.connect.agro.external;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.tests.data;
using trifenix.connect.agro.tests.mock;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.mdm.ts_model;
using Xunit;

namespace trifenix.connect.agro.tests
{
    public class EntitySearchMgmtTest
    {

        /// <summary>
        /// Ingresamos un producto y válidamos los entitySearch creados,
        /// para este producto serían dos dosis, un producto y cuatro waiting harvest.
        /// </summary>
        [Fact]
        public void GetEntitySearchFromInput()
        {
            // assign

            var entitySearchMgm = new EntitySearchMgmt<GeoPointTs>(MockHelper.BaseSearch(), new ImplementMock());


            // action
            var result = entitySearchMgm.GetEntitySearchByInput(AgroData.Product1);




            // assert
            Assert.True(result.Count() == 7
                && result.Count(s => s.index == (int)EntityRelated.PRODUCT) == 1
                && result.Count(s => s.index == (int)EntityRelated.DOSES) == 2
                && result.Count(s => s.index == (int)EntityRelated.WAITINGHARVEST) == 4
                );
        }


        /// <summary>
        /// Verifica que al asignar los ids de las dosis, estas se encuentren en los entitySearch
        /// creados a partir del input.
        /// </summary>
        [Fact]
        public void GetEntitySearchFromInputAssigningIds()
        {
            // assign

            var entitySearchMgm = new EntitySearchMgmt<GeoPointTs>(MockHelper.BaseSearch(), new ImplementMock());


            var prd = AgroData.Product1;

            prd.Doses[0].Id = ConstantGuids.Value[0];
            prd.Doses[1].Id = ConstantGuids.Value[1];
            // action
            var result = entitySearchMgm.GetEntitySearchByInput(prd);

            // assert
            Assert.True(result.Count() == 7
                && result.Count(s => s.index == (int)EntityRelated.PRODUCT) == 1
                && result.Count(s => s.index == (int)EntityRelated.DOSES) == 2
                && result.Count(s => s.index == (int)EntityRelated.WAITINGHARVEST) == 4
                && result.Any(s => s.id.Equals(ConstantGuids.Value[0]) && s.index == (int)EntityRelated.DOSES)
                );
        }

        


        /// <summary>
        /// Obtiene un producto entitySearch desde un elemento de la base de datos (DocumentBase)
        /// </summary>
        [Fact]
        public void GetEntitySearchFromProduct()
        {
            // assign

            var entitySearchMgm = new EntitySearchMgmt<GeoPointTs>(MockHelper.BaseSearch(), new ImplementMock());


            var prd = AgroData.Product1;


            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductInput, Product>());

            var mapperLocal = mapper.CreateMapper();
            var prdModel = mapperLocal.Map<Product>(prd);

            // action
            var result = entitySearchMgm.GetEntitySearch(prdModel);
            // assert
            Assert.True(result.First().index == 12);
        }


        /// <summary>
        /// Verifica que las ids creadas de manera automática en waiting harvest, se hayan asignado a cada waiting harvest 
        /// y que existan en las elementos de relaciones de un entitySearch de tipo dosis.
        /// </summary>
        [Fact]
        public void GetEntitySearchFromDosesCheckCircleId()
        {
            // assign

            var entitySearchMgm = new EntitySearchMgmt<GeoPointTs>(MockHelper.BaseSearch(), new ImplementMock());


            var prd = AgroData.Product1;

            prd.Doses[0].Id = ConstantGuids.Value[0];

            var doseInput = prd.Doses[0];
            var doseModel = new Dose
            {
                Active = doseInput.Active,
                ApplicationDaysInterval = doseInput.ApplicationDaysInterval,
                Default = doseInput.Default,
                DosesApplicatedTo = doseInput.DosesApplicatedTo,
                DosesQuantityMax = doseInput.DosesQuantityMax,
                IdSpecies = doseInput.IdSpecies,
                DosesQuantityMin = doseInput.DosesQuantityMin,
                IdProduct = doseInput.IdProduct,
                NumberOfSequentialApplication = doseInput.NumberOfSequentialApplication,
                HoursToReEntryToBarrack = doseInput.HoursToReEntryToBarrack,
                IdVarieties = doseInput.IdVarieties,
                IdsApplicationTarget = doseInput.IdsApplicationTarget,
                WaitingDaysLabel = doseInput.WaitingDaysLabel,
                WaitingToHarvest = doseInput.WaitingToHarvest.Select(s => new WaitingHarvest { IdCertifiedEntity = s.IdCertifiedEntity, Ppm = s.Ppm, WaitingDays = s.WaitingDays }).ToList(),
                Id = doseInput.Id,
                WettingRecommendedByHectares = doseInput.WettingRecommendedByHectares
            };

            // action
            var result = entitySearchMgm.GetEntitySearch(doseModel);

            var waitingHarvesRelated = result.First(s => s.index == (int)EntityRelated.DOSES).rel.Where(s => s.index == (int)EntityRelated.WAITINGHARVEST).ToList();

            var waitingHarvestEntitySearchs = result.Where(q => q.index == (int)EntityRelated.WAITINGHARVEST).Select(s => s.id).ToList();
            // assert
            Assert.True(waitingHarvesRelated.All(a => waitingHarvestEntitySearchs.Any(o => o.Equals(a.id))));
        }

        /// <summary>
        /// Añade un producto al search, solo se comprueba que no falle y que registre la consulta.
        /// la consulta nunca será igual, por la fecha.
        /// </summary>
        [Fact]
        public void AddProductToSearch()
        {
            // assign

            var entitySearchMgm = new EntitySearchMgmt<GeoPointTs>(MockHelper.BaseSearch(), new ImplementMock());


            var prd = AgroData.Product1;


            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductInput, Product>());

            var mapperLocal = mapper.CreateMapper();
            var prdModel = mapperLocal.Map<Product>(prd);

            // action
            entitySearchMgm.AddDocument(prdModel);
            // assert
            Assert.True(!string.IsNullOrWhiteSpace(Regex.Unescape(entitySearchMgm.Queried.First().Value.First())));
        }
    }
}
