using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.tests.data;
using trifenix.connect.agro.tests.mock;
using trifenix.connect.agro_model_input;
using trifenix.connect.util;
using Xunit;

namespace trifenix.connect.agro.tests
{
    public class BarrackTest
    {
        [Fact]
        public async Task InsertBarrackSuccess() 
        {

            //assign
            var agroManager = MockHelper.AgroManager;


            // especie
            var specieInput = await agroManager.Specie.SaveInput(new SpecieInput
            {
                Abbreviation = "UV",
                Name = "Uva"
            });

            var specie = await agroManager.Specie.Get(specieInput.IdRelated);

            Assert.True(specie.Result.Name.Equals("Uva"));

            // variedad
            var varietyInput = await agroManager.Variety.SaveInput(new VarietyInput
            {
                Abbreviation = "RSD",
                Name = "Rosada",
                IdSpecie = specie.Result.Id
            });

            var variety = await agroManager.Variety.Get(varietyInput.IdRelated);

            Assert.True(variety.Result.Name.Equals("Rosada"));

            //sectores
            var sectorInput = await agroManager.Sector.SaveInput(new SectorInput
            {
                Name = "Cordillera"
            });

            var sector = await agroManager.Sector.Get(sectorInput.IdRelated);

            Assert.True(sector.Result.Name.Equals("Cordillera"));

            //parcelas
            var plotLandInput = await agroManager.PlotLand.SaveInput(new PlotLandInput
            {
                IdSector = sector.Result.Id,
                Name = "Delirio"
            });

            var plotLand = await agroManager.PlotLand.Get(plotLandInput.IdRelated);

            Assert.True(plotLand.Result.Name.Equals("Delirio"));

            //Razon social
            var businessNameInput = await agroManager.BusinessName.SaveInput(new BusinessNameInput
            {
                Name = "TrifenixA",
                Email = "trifenix@trifenix.io",
                Rut = "76955261-2",
                WebPage = "connect.trifenix.io",
                Giro = "agro-fenix",
                Phone = "99999999"
            });

            var businnesName = await agroManager.BusinessName.Get(businessNameInput.IdRelated);

            Assert.True(businnesName.Result.Name.Equals("TrifenixA"));

            //Centro de costo
            var costCenterInput = await agroManager.CostCenter.SaveInput(new CostCenterInput
            {
                Name = "Centro de costo",
                IdBusinessName = businnesName.Result.Id
            });

            var costCenter = await agroManager.CostCenter.Get(costCenterInput.IdRelated);

            Assert.True(costCenter.Result.Name.Equals("Centro de costo"));

            //Raiz
            var rootstockInput = await agroManager.Rootstock.SaveInput(new RootstockInput
            {
                Name = "Royal",
                Abbreviation = "Ryl"
            });

            var rootstock = await agroManager.Rootstock.Get(rootstockInput.IdRelated);

            Assert.True(rootstock.Result.Name.Equals("Royal"));

            //temporada
            var seasonInput = await agroManager.Season.SaveInput(new SeasonInput
            {
                Current = false,
                StartDate = new DateTime(2020, 3, 1),
                EndDate = new DateTime(2022, 3, 1),
                IdCostCenter = costCenter.Result.Id
            });

            var season = await agroManager.Season.Get(seasonInput.IdRelated);

            Assert.True(season.Result.Current.Equals(false));

            //barack
            var barrackInput = new BarrackInput
            {
                Name = "Barrack 3",
                Hectares = 1.1,
                IdPlotLand = plotLand.Result.Id,
                IdPollinator = variety.Result.Id,
                NumberOfPlants = 444,
                IdVariety = variety.Result.Id,
                SeasonId = season.Result.Id,
                IdRootstock = rootstock.Result.Id,
                PlantingYear = 1980,

            };
            var barrackInputTest = await agroManager.Barrack.SaveInput(barrackInput);

            var barrack = await agroManager.Barrack.Get(barrackInputTest.IdRelated);

            Assert.Equal(barrackInput.Name, barrack.Result.Name);

            var compareModel = Mdm.Validation.CompareModel(
                barrackInput,
                barrack.Result,
                new Dictionary<Type, Func<object, IEnumerable<object>>> {
                }
                );
            Assert.True(compareModel);
        }
    }
}
