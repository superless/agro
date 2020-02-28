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

namespace trifenix.agro.functions
{
    public static class CoreFunctions
    {
        [FunctionName("SectorV3")]
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
        public static async Task<IActionResult> PreOrders([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "pro_orders/{id?}")] HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer<PreOrderInput, PreOrder>.SendInternalHttp(req, log, s => s.PreOrders, id);

            return result.JsonResult;
        }












        //    #region v2/executions
        //    [SwaggerIgnore]
        //    [FunctionName("ExecutionsV3")]
        //    public static async Task<IActionResult> Executions([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v3/executions/{idExecution?}")] HttpRequest req, string idExecution, ILogger log)
        //    {
        //        var claims = await Auth.Validate(req);

        //        if (claims == null)
        //            return new UnauthorizedResult();

        //        var manager = await ContainerMethods.AgroManager(claims);



        //        ExtGetContainer<ExecutionOrder> result = null;


        //        switch (req.Method.ToLower())
        //        {
        //            case "get":
        //                result = await manager.ExecutionOrders.GetExecutionOrder(idExecution);

        //                return ContainerMethods.GetJsonGetContainer(result, log);

        //            case "post":
        //                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) => {
        //                    string idOrder = (string)model["idOrder"];
        //                    string idUserApplicator = (string)model["idUserApplicator"];
        //                    string idNebulizer = (string)model["idNebulizer"];
        //                    string[] idProduct = JsonConvert.DeserializeObject<string[]>(((object)model["idProduct"]).ToString());
        //                    double[] quantityByHectare = JsonConvert.DeserializeObject<double[]>(((object)model["quantityByHectare"]).ToString());
        //                    string idTractor = (string)model["idTractor"];
        //                    string commentary = (string)model["commentary"];
        //                    string executionName = (string)model["executionName"];
        //                    return await db.ExecutionOrders.SaveNewExecutionOrder(idOrder, executionName, idUserApplicator, idNebulizer, idProduct, quantityByHectare, idTractor, commentary);
        //                }, claims);
        //            default:
        //                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) => {
        //                    string idOrder = (string)model["idOrder"];
        //                    string idUserApplicator = (string)model["idUserApplicator"];
        //                    string idNebulizer = (string)model["idNebulizer"];
        //                    string[] idProduct = JsonConvert.DeserializeObject<string[]>(((object)model["idProduct"]).ToString());
        //                    double[] quantityByHectare = JsonConvert.DeserializeObject<double[]>(((object)model["quantityByHectare"]).ToString());
        //                    string idTractor = (string)model["idTractor"];
        //                    string executionName = (string)model["executionName"];
        //                    return await db.ExecutionOrders.SaveEditExecutionOrder(idExecution, idOrder, executionName, idUserApplicator, idNebulizer, idProduct, quantityByHectare, idTractor);
        //                }, claims);
        //        }

        //    }
        //    #endregion

        //    #region v2/Executions_ChangeStatus
        //    [FunctionName("Execution_ChangeStatusV3")]
        //    public static async Task<IActionResult> Execution_ChangeStatus([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v3/execution/changeStatus/{idExecution}")] HttpRequest req, string idExecution, ILogger log)
        //    {
        //        ClaimsPrincipal claims = await Auth.Validate(req);
        //        if (claims == null)
        //            return new UnauthorizedResult();

        //        var manager = await ContainerMethods.AgroManager(claims);

        //        return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) => {
        //            var type = (string)model["type"];
        //            var value = (int)model["value"];
        //            var commentary = (string)model["commentary"];
        //            return await manager.ExecutionOrders.SetStatus(idExecution, type, value, commentary);
        //        }, claims);
        //    }
        //    #endregion

        //    #region v2/executionsAddCommentary
        //    [FunctionName("ExecutionsAddCommentaryV3")]
        //    public static async Task<IActionResult> ExecutionsAddCommentary([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v3/execution/add_commentary/{idExecution}")] HttpRequest req, string idExecution, ILogger log)
        //    {
        //        ClaimsPrincipal claims = await Auth.Validate(req);
        //        if (claims == null)
        //            return new UnauthorizedResult();
        //        return await ContainerMethods.ApiPostOperations<ExecutionOrder>(req.Body, log, async (db, model) => {
        //            string commentary = (string)model["commentary"];
        //            return await db.ExecutionOrders.AddCommentaryToExecutionOrder(idExecution, commentary);
        //        }, claims);
        //    }
        //    #endregion
    }
}
