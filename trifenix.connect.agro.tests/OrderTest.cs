using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.tests.data;
using trifenix.connect.agro.tests.mock;
using trifenix.connect.agro.tests;
using trifenix.connect.agro_model_input;
using trifenix.connect.util;
using Xunit;

namespace trifenix.connect.agro.tests
{
    public class OrderTest
    {
        [Fact]
        public async Task InsertOrderSuccess()
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
                new Dictionary<Type, Func<object, IEnumerable<object>>>
                {
                }
                );
            Assert.True(compareModel);
            //Eventos Fenologicos
            var phenologicalEventInput = await agroManager.PhenologicalEvent.SaveInput(new PhenologicalEventInput
            {
                Name = "Lluvia",
                StartDate = new DateTime(2021, 3, 1),
                EndDate = new DateTime(2022, 3, 1)
            });

            var phenologicalEvent = await agroManager.PhenologicalEvent.Get(phenologicalEventInput.IdRelated);

            Assert.True(phenologicalEvent.Result.Name.Equals("Lluvia"));

            //Categoria Ingredientes
            var categoryIngredientResult = await agroManager.IngredientCategory.SaveInput(new IngredientCategoryInput
            {
                Name = "Fertilizantes"
            });

            var categoryIngredient = await agroManager.IngredientCategory.Get(categoryIngredientResult.IdRelated);


            Assert.True(categoryIngredient.Result.Name.Equals("Fertilizantes"));

            //Ingrediente
            var ingredientCategory = categoryIngredient.Result.Id;


            var ingredientInput = await agroManager.Ingredient.SaveInput(new IngredientInput
            {
                idCategory = categoryIngredient.Result.Id,
                Name = "OXIDO CUPROSO"
            });

            var ingredient = await agroManager.Ingredient.Get(ingredientInput.IdRelated);


            Assert.True(ingredient.Result.Name.Equals("OXIDO CUPROSO"));


            // objetivo de la aplicación.
            var targetInput = await agroManager.ApplicationTarget.SaveInput(new ApplicationTargetInput
            {
                Abbreviation = "CB",
                Name = "Cancer Bacterial"

            });

            var target = await agroManager.ApplicationTarget.Get(targetInput.IdRelated);

            Assert.True(target.Result.Name.Equals("Cancer Bacterial"));


            //Order Folder
            var orderFolderInput = await agroManager.OrderFolder.SaveInput(new OrderFolderInput
            {
                IdPhenologicalEvent = phenologicalEvent.Result.Id,
                IdApplicationTarget = target.Result.Id,
                IdSpecie = specie.Result.Id,
                IdIngredient = ingredient.Result.Id,
                IdIngredientCategory = categoryIngredient.Result.Id

            });

            var orderFolder = await agroManager.OrderFolder.Get(orderFolderInput.IdRelated);

            Assert.True(orderFolder.Result.Id.Equals(orderFolder.Result.Id));

            
            //Pre orden
            
            var preOrdenInput = await agroManager.PreOrder.SaveInput(new PreOrderInput
            {
                Name = "Pre orden 1",
                IdIngredient = ingredient.Result.Id,
                OrderFolderId = orderFolder.Result.Id,
                PreOrderType = PreOrderType.DEFAULT,
                BarracksId = new string[] { barrack.Result.Id }
            });


            var preOrden = await agroManager.PreOrder.Get(preOrdenInput.IdRelated);

            Assert.True(preOrden.Result.Name.Equals("Pre orden 1"));

            
            
        }
    }
}
