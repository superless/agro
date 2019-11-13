using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using trifenix.agro.functions.Helper;
using System.Linq;

namespace trifenix.agro.functions
{
    public static class MainAgroFunction
    {
        [FunctionName("PhenologicalEventV2")]
        public static async Task<IActionResult> PhenologicalEventV2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/phenological_events")] HttpRequest req,
            ILogger log)
        {
            if (req.Method.ToLower().Equals("post")) {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var initDate = (DateTime)model["startDate"];
                    var endDate = (DateTime)model["endDate"];

                    
                    return await db.PhenologicalEvents.SaveNewPhenologicalEvent(name, initDate, endDate);
                });
            }

            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var id = (string)model["id"];
                    var name = (string)model["name"];
                    var initDate = (DateTime)model["startDate"];
                    var endDate = (DateTime)model["endDate"];

                    return await db.PhenologicalEvents.SaveEditPhenologicalEvent(id, name, initDate, endDate);
                });
            }
            var manager = await ContainerMethods.AgroManager();
            var result = await manager.PhenologicalEvents.GetPhenologicalEvents();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }

        [FunctionName("SeasonV2")]
        public static async Task<IActionResult> SeasonV2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/seasons")] HttpRequest req,
            ILogger log)
        {
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    
                    var initDate = (DateTime)model["startDate"];
                    var endDate = (DateTime)model["endDate"];


                    return await db.Seasons.SaveNewSeason(initDate, endDate);
                });
            }

            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var id = (string)model["id"];
                    
                    var initDate = (DateTime)model["startDate"];
                    var endDate = (DateTime)model["endDate"];
                    var current = (bool)model["current"];

                    return await db.Seasons.SaveEditSeason(id, initDate, endDate, current);
                });
            }

            var manager = await ContainerMethods.AgroManager();
            var result = await manager.Seasons.GetSeasons();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }

        [FunctionName("SpecieV2")]
        public static async Task<IActionResult> SpecieV2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/species")] HttpRequest req,
            ILogger log)
        {
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var abbreviation = (string)model["abbreviation"];
                    return await db.Species.SaveNewSpecie(name, abbreviation);
                });
            }

            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var id = (string)model["id"];

                    var name = (string)model["name"];
                    var abbreviation = (string)model["abbreviation"];
                    return await db.Species.SaveEditSpecie(id, name, abbreviation);
                });
            }

            var manager = await ContainerMethods.AgroManager();
            var result = await manager.Species.GetSpecies();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }

        [FunctionName("CategoryIngredientsV2")]
        public static async Task<IActionResult> CategoryIngredientsV2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/ingredient_categories")] HttpRequest req,
            ILogger log)
        {
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    return await db.IngredientCategories.SaveNewIngredientCategory(name);
                });
            }

            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var id = (string)model["id"];

                    var name = (string)model["name"];

                    return await db.IngredientCategories.SaveEditIngredientCategory(id, name);
                });
            }

            var manager = await ContainerMethods.AgroManager();
            var result = await manager.IngredientCategories.GetIngredientCategories();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }


        [FunctionName("IngredientsV2")]
        public static async Task<IActionResult> IngredientsV2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/ingredients")] HttpRequest req,
            ILogger log)
        {
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var idCategory = (string)model["idCategory"];
                    return await db.Ingredients.SaveNewIngredient(name, idCategory);
                });
            }

            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var id = (string)model["id"];

                    var name = (string)model["name"];
                    var idCategory = (string)model["idCategory"];

                    return await db.Ingredients.SaveEditIngredient(id, name, idCategory);
                });
            }

            var manager = await ContainerMethods.AgroManager();
            var result = await manager.Ingredients.GetIngredients();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }


        [FunctionName("TargetV2")]
        public static async Task<IActionResult> TargetV2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/targets")] HttpRequest req,
            ILogger log)
        {
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];

                    return await db.ApplicationTargets.SaveNewApplicationTarget(name);
                });
            }

            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var id = (string)model["id"];

                    var name = (string)model["name"];


                    return await db.ApplicationTargets.SaveEditApplicationTarget(id, name);
                });
            }

            var manager = await ContainerMethods.AgroManager();
            var result = await manager.ApplicationTargets.GetAplicationsTarget();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }

        [FunctionName("OrderFolder")]
        public static async Task<IActionResult> OrderFolder(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/order_folders/{parameter?}")] HttpRequest req, string parameter,
            ILogger log)
        {
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var idPhenologicalEvent = (string)model["idPhenologicalEvent"];
                    var idApplicationTarget = (string)model["idApplicationTarget"];
                    var categoryId = (string)model["idCategory"];
                    var idSpecie = (string)model["idSpecie"];
                    var idIngredient = (string)model["idIngredient"];
                    return await db.OrderFolder.SaveNewOrderFolder(idPhenologicalEvent, idApplicationTarget, categoryId, idSpecie, idIngredient);
                });
            }

            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var id = (string)model["id"];
                    var idPhenologicalEvent = (string)model["idPhenologicalEvent"];
                    var idApplicationTarget = (string)model["idApplicationTarget"];
                    var idCategory = (string)model["idCategory"];
                    var idSpecie = (string)model["idSpecie"];
                    var idIngredient = (string)model["idIngredient"];
                    


                    return await db.OrderFolder.SaveEditOrderFolder(id, idPhenologicalEvent, idApplicationTarget, idCategory, idSpecie, idIngredient);
                });
            }

            if (!string.IsNullOrWhiteSpace(parameter)) {
                var managerLocal = await ContainerMethods.AgroManager();
                var resultLocal = await managerLocal.OrderFolder.GetOrderFolder(parameter);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);

            }

            var manager = await ContainerMethods.AgroManager();
            var result = await manager.OrderFolder.GetOrderFolders();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }

        [FunctionName("UploadEvent")]
        public static async Task<IActionResult> UploadEvent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "v2/notification_events/{parameter?}")] HttpRequest req, string parameter,
            ILogger log)
        {
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {

                    var newModel = model["_parts"][0][1];
                    var idPhenologicalEvent = (string)newModel["idPhenologicalEvent"];
                    var description = (string)newModel["description"];
                    var base64 = (string)newModel["base64"];
                    var barrack = (string)newModel["idBarrack"];


                    return await db.NotificationEvents.SaveNewNotificationEvent(barrack, idPhenologicalEvent, base64, description);
                });
            }

            

            if (!string.IsNullOrWhiteSpace(parameter))
            {
                var managerLocal = await ContainerMethods.AgroManager();
                var resultLocal = await managerLocal.NotificationEvents.GetEvent(parameter);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);

            }

            var manager = await ContainerMethods.AgroManager();
            var result = await manager.NotificationEvents.GetEvents();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }


        [FunctionName("SectorV2")]
        public static async Task<IActionResult> Sector(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/sectors/{parameter?}")] HttpRequest req, string parameter,
            ILogger log)
        {
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                   
                    var name = (string)model["name"];
                    return await db.Sectors.SaveNewSector(name);
                });
            }

            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var id = (string)model["id"];
                    var name = (string)model["name"];




                    return await db.Sectors.SaveEditSector(id, name);
                });
            }

            if (!string.IsNullOrWhiteSpace(parameter))
            {
                var managerLocal = await ContainerMethods.AgroManager();
                var resultLocal = await managerLocal.Sectors.GetSector(parameter);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);

            }

            var manager = await ContainerMethods.AgroManager();
            var result = await manager.Sectors.GetSectors();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }

        [FunctionName("PlotLandsV2")]
        public static async Task<IActionResult> PlotLandsV2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/plotlands/{parameter?}")] HttpRequest req, string parameter,
            ILogger log)
        {
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {

                    var name = (string)model["name"];
                    var idSector = (string)model["idSector"];
                    return await db.PlotLands.SaveNewPlotLand(name, idSector);
                });
            }

            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var id = (string)model["id"];
                    var name = (string)model["name"];
                    var idSector = (string)model["idSector"];



                    return await db.PlotLands.SaveEditPlotLand(id, name, idSector);
                });
            }

            if (!string.IsNullOrWhiteSpace(parameter))
            {
                var managerLocal = await ContainerMethods.AgroManager();
                var resultLocal = await managerLocal.PlotLands.GetPlotLand(parameter);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);

            }

            var manager = await ContainerMethods.AgroManager();
            var result = await manager.PlotLands.GetPlotLands();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }

        [FunctionName("VarietiesV2")]
        public static async Task<IActionResult> VarietiesV2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/varieties/{parameter?}")] HttpRequest req, string parameter,
            ILogger log)
        {
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {

                    var name = (string)model["name"];
                    var idSpecie = (string)model["idSpecie"];
                    var abbreviation = (string)model["abbreviation"];
                    return await db.Varieties.SaveNewVariety(name, abbreviation, idSpecie);
                });
            }

            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var id = (string)model["id"];
                    var name = (string)model["name"];
                    var idSpecie = (string)model["idSpecie"];
                    var abbreviation = (string)model["abbreviation"];



                    return await db.Varieties.SaveEditVariety(id, name, abbreviation, idSpecie);
                });
            }

            if (!string.IsNullOrWhiteSpace(parameter))
            {
                var managerLocal = await ContainerMethods.AgroManager();
                var resultLocal = await managerLocal.Varieties.GetVariety(parameter);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);

            }

            var manager = await ContainerMethods.AgroManager();
            var result = await manager.Varieties.GetVarieties();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }

        [FunctionName("BarracksV2")]
        public static async Task<IActionResult> BarracksV2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/barracks/{parameter?}")] HttpRequest req, string parameter,
            ILogger log)
        {
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {

                    var name = (string)model["name"];
                    var numberOfPlants = (int)model["numberOfPlants"];
                    var plantingYear = (int)model["plantingYear"];
                    var hectares = float.Parse((string)model["hectares"]);
                    var idPlotland = (string)model["idPlotland"];
                    var idVariety = (string)model["idVariety"];
                    var idPollinator = (string)model["idPollinator"];
                    return await db.Barracks.SaveNewBarrack(name, idPlotland, hectares, plantingYear, idVariety, numberOfPlants, idPollinator);
                });
            }

            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var id = (string)model["id"];
                    var name = (string)model["name"];
                    var numberOfPlants = (int)model["numberOfPlants"];
                    var plantingYear = (int)model["plantingYear"];
                    var hectares = float.Parse((string)model["hectares"]);
                    var idPlotland = (string)model["idPlotland"];
                    var idVariety = (string)model["idVariety"];
                    var idPollinator = (string)model["idPollinator"];



                    return await db.Barracks.SaveEditBarrack(id, name, idPlotland, hectares, plantingYear, idVariety, numberOfPlants, idPollinator);
                });
            }

            if (!string.IsNullOrWhiteSpace(parameter))
            {
                var managerLocal = await ContainerMethods.AgroManager();
                var resultLocal = await managerLocal.Barracks.GetBarrack(parameter);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);

            }

            var manager = await ContainerMethods.AgroManager();
            var result = await manager.Barracks.GetBarracks();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }

        [FunctionName("PhenologicalPreOrders")]
        public static async Task<IActionResult> PhenologicalPreOrders(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/phenological_preorders/{parameter?}")] HttpRequest req, string parameter,
            ILogger log)
        {
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {

                    var name = (string)model["name"];
                    var idFolder = (string)model["idOrderFolder"];
                    var arr = (string)model["idBarracks"].ToString();
                    var idBarracks = JsonConvert.DeserializeObject<string[]>(arr);

                    return await db.PhenologicalPreOrders.SaveNewPhenologicalPreOrder(name, idFolder, idBarracks.ToList());
                });
            }

            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var id = (string)model["id"];
                    var name = (string)model["name"];
                    var idFolder = (string)model["idOrderFolder"];
                    var arr = (string)model["idBarracks"].ToString();
                    var idBarracks = JsonConvert.DeserializeObject<string[]>(arr);



                    return await db.PhenologicalPreOrders.SaveEditPhenologicalPreOrder(id, name, idFolder, idBarracks.ToList());
                });
            }

            if (!string.IsNullOrWhiteSpace(parameter))
            {
                var managerLocal = await ContainerMethods.AgroManager();
                var resultLocal = await managerLocal.PhenologicalPreOrders.GetPhenologicalPreOrder(parameter);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);

            }

            var manager = await ContainerMethods.AgroManager();
            var result = await manager.PhenologicalPreOrders.GetPhenologicalPreOrders();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }






    }
}
