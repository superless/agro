using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using trifenix.agro.functions.Helper;
using trifenix.agro.db.model.agro;

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

            var result = await ContainerMethods.AgroManager.PhenologicalEvents.GetPhenologicalEvents();
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

            var result = await ContainerMethods.AgroManager.Seasons.GetSeasons();
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

            var result = await ContainerMethods.AgroManager.Species.GetSpecies();
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

            var result = await ContainerMethods.AgroManager.IngredientCategories.GetIngredientCategories();
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
                    var idCategory = (string)model["categoryId"];
                    return await db.Ingredients.SaveNewIngredient(name, idCategory);
                });
            }

            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var id = (string)model["id"];

                    var name = (string)model["name"];
                    var idCategory = (string)model["categoryId"];

                    return await db.Ingredients.SaveEditIngredient(id, name, idCategory);
                });
            }

            var result = await ContainerMethods.AgroManager.Ingredients.GetIngredients();
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

            var result = await ContainerMethods.AgroManager.ApplicationTargets.GetAplicationsTarget();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }



    }
}
