using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using trifenix.agro.functions.mantainers;
using trifenix.agro.model.external.Input;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.functions.Helper;
using System.Net;
using trifenix.agro.model.external;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using trifenix.agro.swagger.model.input;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using trifenix.agro.external.operations.helper;
using System.Collections.Generic;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.db.model;

namespace trifenix.agro.functions
{
    /// <summary>
    /// Funciones 
    /// </summary>
    public static class CoreFunctions
    {

        /// <summary>
        /// Login, donde usa usuario y contraseña para obtener el token.
        /// </summary>
        /// <param name="req">cabecera que debe incluir el modelo de entrada </param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("login")]
        [RequestHttpHeader("Authorization", isRequired: true)]

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            [RequestBodyType(typeof(LoginInput), "Nombre de usuario y contraseña")]HttpRequest req,
            ILogger log)
        {

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic credenciales = JsonConvert.DeserializeObject(requestBody);

            string clientId = "a81f0ad4-912b-46d3-ba3e-7bf605693242";
            string scope = "https://sprint3-jhm.trifenix.io/App.access";
            string clientSecret = "gUjIa4F=NXlAwwMCF2j2SFMMj3?m@=FM";
            string username = (string)credenciales["username"];
            string password = (string)credenciales["password"];
            string grantType = "password";
            string endPoint = "https://login.microsoftonline.com/jhmad.onmicrosoft.com/oauth2/v2.0/token";

            HttpClient client = new HttpClient();
            var parametros = new Dictionary<string, string> {
                {"client_id",clientId},
                {"scope",scope},
                {"client_secret",clientSecret},
                {"username",username},
                {"password",password},
                {"grant_type",grantType}
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, endPoint);
            requestMessage.Content = new FormUrlEncodedContent(parametros);
            var response = await client.SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(responseBody);
            client.Dispose();
            string accessToken = json.access_token;
            return ContainerMethods.GetJsonGetContainer(OperationHelper.GetElement(accessToken), log);
        }


        [FunctionName("Sector")]

        public static async Task<IActionResult> SectorV3([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "sectors/{id?}")] HttpRequest req, string id, ILogger log)
        {

            
            var result = await GenericMantainer<SectorInput,Sector>.SendInternalHttp(req, log, s => s.Sectors, id);
            return result.JsonResult;


        }

        [FunctionName("PlotLandsV3")]
        public static async Task<IActionResult> PlotLandsV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "plotlands/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<PlotLandInput, PlotLand>.SendInternalHttp(req, log, s => s.PlotLands, id);
            return result.JsonResult;
        }

        [FunctionName("SpecieV3")]
        public static async Task<IActionResult> Speciev3([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "species/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<SpecieInput, Specie>.SendInternalHttp(req, log, s => s.Species, id);

            return result.JsonResult;
        }

        [FunctionName("VarietiesV3")]
        public static async Task<IActionResult> VarietiesV3([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "varieties/{id?}")] HttpRequest req, string id, ILogger log) {
            
            var result = await GenericMantainer < VarietyInput, Variety>.SendInternalHttp(req, log, s => s.Varieties, id);

            return result.JsonResult;
        }

        [FunctionName("TargetV3")]
        public static async Task<IActionResult> TargetV3([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "targets/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<TargetInput, ApplicationTarget>.SendInternalHttp(req, log, s => s.ApplicationTargets,id);

            return result.JsonResult;
        }


        [FunctionName("PhenologicalEventV3")]
        public static async Task<IActionResult> PhenologicalEventV3([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "phenological_events/{id?}")] HttpRequest req, string id, ILogger log) {

            var result = await GenericMantainer< PhenologicalEventInput, PhenologicalEvent>.SendInternalHttp(req, log, s => s.PhenologicalEvents, id);

            return result.JsonResult;
        }


        [FunctionName("CertifiedEntityV3")]
        public static async Task<IActionResult> CertifiedEntityV3([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "certified_entities/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<CertifiedEntityInput, CertifiedEntity>.SendInternalHttp(req, log, s => s.CertifiedEntities, id);

            return result.JsonResult;
        }


        [FunctionName("CategoryIngredientsV3")]
        public static async Task<IActionResult> CategoryIngredientsV3([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "ingredient_categories/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<IngredientCategoryInput, IngredientCategory>.SendInternalHttp(req, log, s => s.IngredientCategories, id);

            return result.JsonResult;
        }

        [FunctionName("IngredientsV3")]
        public static async Task<IActionResult> IngredientsV3([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "ingredients/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<IngredientInput, Ingredient>.SendInternalHttp(req, log, s => s.Ingredients, id);

            return result.JsonResult;
        }

        [FunctionName("ProductV3")]
        public static async Task<IActionResult> ProductV3([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/products/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<ProductInput, Product>.SendInternalHttp(req, log, s => s.Products, id);

            return result.JsonResult;

        }


        [FunctionName("Roles")]
        public static async Task<IActionResult> Roles([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "roles/{id?}")] HttpRequest req, ILogger log, string id)
        {
            var result = await GenericMantainer < RoleInput, Role>.SendInternalHttp(req, log, s => s.Roles, id);

            return result.JsonResult;
        }


        [FunctionName("Jobs")]
        public static async Task<IActionResult> Jobs([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "jobs/{id?}")] HttpRequest req, ILogger log, string id)
        {
            var result = await GenericMantainer<JobInput, Job>.SendInternalHttp(req, log, s => s.Jobs, id);

            return result.JsonResult;
        }

        [FunctionName("Users")]
        public static async Task<IActionResult> Users([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "users/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<UserApplicatorInput, UserApplicator>.SendInternalHttp(req, log, s => s.Users, id);

            return result.JsonResult;
        }

        [FunctionName("Nebulizers")]
        public static async Task<IActionResult> Nebulizers([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "nebulizers/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<NebulizerInput, Nebulizer>.SendInternalHttp(req, log, s => s.Nebulizers, id);

            return result.JsonResult;
        }


        [FunctionName("Tractors")]
        public static async Task<IActionResult> Tractors([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "tractors/{id?}")] HttpRequest req, string id, ILogger log)
        {

            var result = await GenericMantainer<TractorInput, Tractor >.SendInternalHttp(req, log, s => s.Tractors, id);

            return result.JsonResult;
        }

        [FunctionName("BusinessName")]
        public static async Task<IActionResult> BusinessName([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "businessNames/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<BusinessNameInput, BusinessName>.SendInternalHttp(req, log, s => s.BusinessNames, id);

            return result.JsonResult;
        }

        [FunctionName("CostCenter")]
        public static async Task<IActionResult> CostCenter([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "costCenters/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<CostCenterInput, CostCenter>.SendInternalHttp(req, log, s => s.CostCenters, id);

            return result.JsonResult;
        }

        [FunctionName("SeasonV3")]
        public static async Task<IActionResult> SeasonV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "seasons/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<CostCenterInput, CostCenter>.SendInternalHttp(req, log, s => s.CostCenters, id);

            return result.JsonResult;
        }


        [FunctionName("RootstockV2")]
        public static async Task<IActionResult> RootstockV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "rootstock/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<RootStockInput, Rootstock>.SendInternalHttp(req, log, s => s.Rootstock, id);
            return result.JsonResult;
        }

        [FunctionName("OrderFolder")]
        public static async Task<IActionResult> OrderFolder([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "order_folders/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<OrderFolderInput, OrderFolder>.SendInternalHttp(req, log, s => s.OrderFolder, id);
            return result.JsonResult;
        }

        [FunctionName("Barracks")]
        public static async Task<IActionResult> BarracksV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "barracks/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<BarrackInput, Barrack>.SendInternalHttp(req, log, s => s.Barracks, id);
            return result.JsonResult;
        }


        [FunctionName("NotificationEvents")]
        public static async Task<IActionResult> NotificationEvents([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "notification_events/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<NotificationEventInput, NotificationEvent>.SendInternalHttp(req, log, s => s.NotificationEvents, id);
            return result.JsonResult;
        }


        [FunctionName("PreOrders")]
        public static async Task<IActionResult> PreOrders([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "pre_orders/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<PreOrderInput, PreOrder>.SendInternalHttp(req, log, s => s.PreOrders, id);

            return result.JsonResult;
        }

        [FunctionName("Orders")]
        public static async Task<IActionResult> Orders([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "orders/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<ApplicationOrderInput, ApplicationOrder>.SendInternalHttp(req, log, s => s.ApplicationOrders, id);

            return result.JsonResult;
        }

        [FunctionName("Executions")]
        public static async Task<IActionResult> ExecutionOrders([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "executions/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<ExecutionOrderInput, ExecutionOrder>.SendInternalHttp(req, log, s => s.ExecutionOrders, id);

            return result.JsonResult;
        }

        [FunctionName("ExecutionStatus")]
        public static async Task<IActionResult> ExecutionOrdersStatus([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "executions_status/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<ExecutionOrderStatusInput, ExecutionOrderStatus>.SendInternalHttp(req, log, s => s.ExecutionStatus, id);

            return result.JsonResult;
        }









    }
}
