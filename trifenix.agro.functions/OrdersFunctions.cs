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
using trifenix.util.extensions;
using trifenix.agro.model.external.Helper;
using trifenix.agro.model.external;

namespace trifenix.agro.functions
{
    public static class OrdersFunctions
    {

        [FunctionName("Ingredient")]
        public static async Task<IActionResult> Ingredients(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "v1/ingredients")] HttpRequest req,
            ILogger log
            )
        {
            var db = ContainerMethods.ContainerDbApplication;
            if (req.Method.ToLower().Equals("post"))
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                dynamic result = JsonConvert.DeserializeObject(requestBody);

                var name = (string)result.name;

                var idCategory = (string)result.uuid;

                var resultDb = await db.SaveActiveIngredient(name,idCategory);

                return ContainerMethods.GetJsonPostContainer(resultDb, log);
            }

            var getResult = await db.GetIngredients();

            return ContainerMethods.GetJsonGetContainer(getResult, log);
        }


        [FunctionName("Category")]
        public static async Task<IActionResult> Categories(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "v1/categories")] HttpRequest req,
            ILogger log)
        {
            var db = ContainerMethods.ContainerDbApplication;
            if (req.Method.ToLower().Equals("post"))
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                dynamic result = JsonConvert.DeserializeObject(requestBody);

                var name = (string)result.name;

                var resultDb = await db.SaveActiveIngredientCategory(name);

                return ContainerMethods.GetJsonPostContainer(resultDb, log);
            }

            var getResult = await db.GetCategories();

            return ContainerMethods.GetJsonGetContainer(getResult, log);
        }


        [FunctionName("Variety")]
        public static async Task<IActionResult> Varieties(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "v1/varieties")] HttpRequest req,
            ILogger log
            )
        {
            var db = ContainerMethods.ContainerDbApplication;
            if (req.Method.ToLower().Equals("post"))
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                dynamic result = JsonConvert.DeserializeObject(requestBody);

                var name = (string)result.name;

                var abbreviation = (string)result.abbreviation;


                var idSpecie = (string)result.uuid;

                var resultDb = await db.SaveVariety(name,abbreviation, idSpecie);

                return ContainerMethods.GetJsonPostContainer(resultDb, log);
            }

            var getResult = await db.GetVarieties();

            return ContainerMethods.GetJsonGetContainer(getResult, log);
        }


        [FunctionName("Specie")]
        public static async Task<IActionResult> Species(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "v1/species")] HttpRequest req,
            ILogger log)
        {
            var db = ContainerMethods.ContainerDbApplication;
            if (req.Method.ToLower().Equals("post"))
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                dynamic result = JsonConvert.DeserializeObject(requestBody);

                var name = (string)result.name;

                var abbreviation = (string)result.abbreviation;

                var resultDb = await db.SaveSpecie(name, abbreviation);
                
                return ContainerMethods.GetJsonPostContainer(resultDb, log);
            }

            var getResult = await db.GetSpecies();

            return ContainerMethods.GetJsonGetContainer(getResult, log);
        }


        [FunctionName("PhenologicalEvent")]
        public static async Task<IActionResult> PhenologicalEvent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "v1/phenological_events")] HttpRequest req,
            ILogger log)
        {
            var db = ContainerMethods.ContainerDbApplication;
            if (req.Method.ToLower().Equals("post"))
            {

                
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                dynamic result = JsonConvert.DeserializeObject(requestBody);

                var taskName = (string)result["name"];

                var initDate = (DateTime)result.startDate;

                var savePhenological = db.SavePhenologicalEvent(taskName, initDate);
                
                var resultDb = await savePhenological;

                
                return ContainerMethods.GetJsonPostContainer(resultDb, log);


            }

            var getResult = await db.GetPhenologicalEvents();

            return ContainerMethods.GetJsonGetContainer(getResult, log);
        }


        

        [FunctionName("ApplicationPurpose")]
        public static async Task<IActionResult> ApplicationPurpose([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "v1/application_purpose")] HttpRequest req,
            ILogger log
            )
        {
            var db = ContainerMethods.ContainerDbApplication;
            if (req.Method.ToLower().Equals("post"))
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                dynamic result = JsonConvert.DeserializeObject(requestBody);

                var name = (string)result.name;

                

                var resultDb = await db.SaveApplicationPurpose(name);

                return ContainerMethods.GetJsonPostContainer(resultDb, log);
            }

            var getResults = await db.GetApplicationPurposes();

            return ContainerMethods.GetJsonGetContainer(getResults, log);
        }

        [FunctionName("Season")]
        public static async Task<IActionResult> Seasons([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "v1/seasons/{parameter?}")] HttpRequest req,
            string parameter,
            ILogger log
            )
        {
            var db = ContainerMethods.ContainerDbApplication;
            if (req.Method.ToLower().Equals("post"))
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                dynamic result = JsonConvert.DeserializeObject(requestBody);

                var start = (DateTime)result.start;
                var end = (DateTime)result.end;
                var resultDb = await db.SaveSeason(start, end);

                return ContainerMethods.GetJsonPostContainer(resultDb, log);
            }
            if (!string.IsNullOrWhiteSpace(parameter) && parameter.ToLower().Equals("exists"))
            {
                var getResults = await db.CurrentSeasonExists();

                return ContainerMethods.GetJsonGetContainer(getResults, log);
            }

            if(!string.IsNullOrWhiteSpace(parameter) && parameter.ToLower().Equals("current"))
            {
                var getResults = await db.GetCurrentSeason();

                return ContainerMethods.GetJsonGetContainer(getResults, log);
            }

            return new JsonResult(null);
        }

        [FunctionName("Field")]
        public static async Task<IActionResult> Fields([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "v1/fields")] HttpRequest req,
           ILogger log
           )
        {
            var db = ContainerMethods.ContainerDbApplication;
            if (req.Method.ToLower().Equals("post"))
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                dynamic result = JsonConvert.DeserializeObject(requestBody);

                var name = (string)result.name;
                var abbreviation = (string)result.abbreviation;
                var hectares = (double)result.hectares;
                var varieties = JsonConvert.DeserializeObject<string[]>(result.varieties.ToString()); 
                var precessor = DynamicExtension.HasProperty(result, "precessor") ? (string)result.precessor : null;


                var resultDb = await db.SaveField(name, abbreviation, hectares, varieties, precessor);

                return ContainerMethods.GetJsonPostContainer(resultDb, log);
            }
            var getResult = await db.GetFields();

            return ContainerMethods.GetJsonGetContainer(getResult, log);

            
        }

        [FunctionName("DateOrder")]
        public static async Task<IActionResult> DateOrders([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "v1/date_orders")] HttpRequest req,
           ILogger log
           )
        {
            var db = ContainerMethods.ContainerDbApplication;
            if (req.Method.ToLower().Equals("post"))
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                dynamic result = JsonConvert.DeserializeObject(requestBody);

                var name = (string)result.name;
                var initDate = (DateTime)result.initDate;
                var idApplicationPurpose = (string)result.idApplicationPurpose;
                var duration = Convert.ToInt32((string)result.duration);

                var applications = JsonConvert.DeserializeObject<ExternalApplication[]>(result.applications.ToString());

                var resultDb = await db.SaveDateOrder(name, initDate, idApplicationPurpose, duration, applications);

                return ContainerMethods.GetJsonPostContainer(resultDb, log);
            }


            return new UnauthorizedResult();

        }

        [FunctionName("ContinuedOrder")]
        public static async Task<IActionResult> ContinuedOrders([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "v1/continued_orders")] HttpRequest req,
           ILogger log
           )
        {
            var db = ContainerMethods.ContainerDbApplication;
            if (req.Method.ToLower().Equals("post"))
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                dynamic result = JsonConvert.DeserializeObject(requestBody);

                var name = (string)result.name;
                var precessor = (string)result.precessor;
                var idApplicationPurpose = (string)result.idApplicationPurpose;
                var duration = Convert.ToInt32((string)result.duration);

                var applications = JsonConvert.DeserializeObject<ExternalApplication[]>(result.applications.ToString());

                var resultDb = await db.SaveContinuedOrder(name, precessor, idApplicationPurpose, duration, applications);

                return ContainerMethods.GetJsonPostContainer(resultDb, log);
            }


            return new UnauthorizedResult();

        }


        [FunctionName("PhenologicalOrder")]
        public static async Task<IActionResult> PhenologicalOrders([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "v1/phenological_orders")] HttpRequest req,
           ILogger log
           )
        {
            var db = ContainerMethods.ContainerDbApplication;
            if (req.Method.ToLower().Equals("post"))
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                dynamic result = JsonConvert.DeserializeObject(requestBody);

                var name = (string)result.name;
                var idPhenologicalEvent = (string)result.idPhenologicalEvent;
                var idApplicationPurpose = (string)result.idApplicationPurpose;
                var duration = Convert.ToInt32((string)result.duration);

                var applications = JsonConvert.DeserializeObject<ExternalApplication[]>(result.applications.ToString());

                var resultDb = await db.SavePhenologicalOrder(name,idPhenologicalEvent, idApplicationPurpose, duration, applications);

                return ContainerMethods.GetJsonPostContainer(resultDb, log);
            }


            return new UnauthorizedResult();


        }


    }
}
