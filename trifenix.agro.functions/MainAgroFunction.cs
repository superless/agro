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
using trifenix.agro.db.model.agro.enums;
using trifenix.agro.model.external.Input;
using trifenix.agro.db.model.agro;
using trifenix.agro.email.operations;
using trifenix.agro.model.external.output;
using System.Security.Claims;

namespace trifenix.agro.functions
{
    public static class MainAgroFunction
    {
        private static readonly Email email = new Email();

        //otra prueba


        // cristian very good job adding id for every query. 
        #region v2/phenological_events
        [FunctionName("PhenologicalEventV2")]
        public static async Task<IActionResult> PhenologicalEventV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/phenological_events/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var initDate = (DateTime)model["startDate"];
                    var endDate = (DateTime)model["endDate"];
                    return await db.PhenologicalEvents.SaveNewPhenologicalEvent(name, initDate, endDate);
                },claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var initDate = (DateTime)model["startDate"];
                    var endDate = (DateTime)model["endDate"];
                    return await db.PhenologicalEvents.SaveEditPhenologicalEvent(id, name, initDate, endDate);
                }, claims);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.PhenologicalEvents.GetPhenologicalEvents();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/seasons
        [FunctionName("SeasonV2")]
        public static async Task<IActionResult> SeasonV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/seasons/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var initDate = (DateTime)model["startDate"];
                    var endDate = (DateTime)model["endDate"];
                    return await db.Seasons.SaveNewSeason(initDate, endDate);
                }, claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var initDate = (DateTime)model["startDate"];
                    var endDate = (DateTime)model["endDate"];
                    var current = (bool)model["current"];
                    return await db.Seasons.SaveEditSeason(id, initDate, endDate, current);
                }, claims);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.Seasons.GetSeasons();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/specie
        [FunctionName("SpecieV2")]
        public static async Task<IActionResult> SpecieV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/species/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var abbreviation = (string)model["abbreviation"];
                    return await db.Species.SaveNewSpecie(name, abbreviation);
                }, claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var abbreviation = (string)model["abbreviation"];
                    return await db.Species.SaveEditSpecie(id, name, abbreviation);
                }, claims);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.Species.GetSpecies();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/rootstock
        [FunctionName("RootstockV2")]
        public static async Task<IActionResult> RootstockV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/rootstock/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var abbreviation = (string)model["abbreviation"];
                    return await db.Rootstock.SaveNewRootstock(name, abbreviation);
                }, claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var abbreviation = (string)model["abbreviation"];
                    return await db.Rootstock.SaveEditRootstock(id, name, abbreviation);
                }, claims);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.Rootstock.GetRootstocks();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/certified_entities
        [FunctionName("CertifiedEntity")]
        public static async Task<IActionResult> CertifiedEntity([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/certified_entities/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var abbreviation = (string)model["abbreviation"];
                    return await db.CertifiedEntities.SaveNewCertifiedEntity(name, abbreviation);
                }, claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var abbreviation = (string)model["abbreviation"];
                    return await db.CertifiedEntities.SaveEditCertifiedEntity(id, name, abbreviation);
                }, claims);
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                var managerLocal = await ContainerMethods.AgroManager(claims);
                var resultLocal = await managerLocal.CertifiedEntities.GetCertifiedEntity(id);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.CertifiedEntities.GetCertifiedEntities();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/ingredient_categories
        [FunctionName("CategoryIngredientsV2")]
        public static async Task<IActionResult> CategoryIngredientsV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/ingredient_categories/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    return await db.IngredientCategories.SaveNewIngredientCategory(name);
                }, claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    return await db.IngredientCategories.SaveEditIngredientCategory(id, name);
                }, claims);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.IngredientCategories.GetIngredientCategories();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/ingredients
        [FunctionName("IngredientsV2")]
        public static async Task<IActionResult> IngredientsV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/ingredients/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var idCategory = (string)model["idCategory"];
                    return await db.Ingredients.SaveNewIngredient(name, idCategory);
                }, claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var idCategory = (string)model["idCategory"];
                    return await db.Ingredients.SaveEditIngredient(id, name, idCategory);
                }, claims);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.Ingredients.GetIngredients();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/targets
        [FunctionName("TargetV2")]
        public static async Task<IActionResult> TargetV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/targets/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    return await db.ApplicationTargets.SaveNewApplicationTarget(name);
                }, claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    return await db.ApplicationTargets.SaveEditApplicationTarget(id, name);
                }, claims);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.ApplicationTargets.GetAplicationsTarget();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/order_folders
        [FunctionName("OrderFolder")]
        public static async Task<IActionResult> OrderFolder([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/order_folders/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
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
                }, claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var idPhenologicalEvent = (string)model["idPhenologicalEvent"];
                    var idApplicationTarget = (string)model["idApplicationTarget"];
                    var idCategory = (string)model["idCategory"];
                    var idSpecie = (string)model["idSpecie"];
                    var idIngredient = (string)model["idIngredient"];
                    return await db.OrderFolder.SaveEditOrderFolder(id, idPhenologicalEvent, idApplicationTarget, idCategory, idSpecie, idIngredient);
                }, claims);
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                var managerLocal = await ContainerMethods.AgroManager(claims);
                var resultLocal = await managerLocal.OrderFolder.GetOrderFolder(id);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.OrderFolder.GetOrderFolders();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/products
        [FunctionName("Product")]
        public static async Task<IActionResult> Product([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/products/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations<string>(req.Body, log, async (db, model) =>
                {
                    var commercialName = (string)model["commercialName"];
                    var idActiveIngredient = (string)model["idActiveIngredient"];
                    var brand = (string)model["brand"];
                    var measureType = (MeasureType)Convert.ToInt32(model["measureType"]);
                    var quantity = (int)model["quantity"];
                    var kindOfBottle = (KindOfProductContainer)Convert.ToInt32(model["kindOfBottle"]);
                    var dosesStr = ((object)model["doses"])?.ToString();
                    var doses = !string.IsNullOrWhiteSpace(dosesStr) ? JsonConvert.DeserializeObject<DosesInput[]>(dosesStr) : null;
                    return await db.Products.CreateProduct(commercialName, idActiveIngredient, brand, doses, measureType, quantity, kindOfBottle);
                }, claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations<Product>(req.Body, log, async (db, model) =>
                {
                    var commercialName = (string)model["commercialName"];
                    var idActiveIngredient = (string)model["idActiveIngredient"];
                    var brand = (string)model["brand"];
                    var measureType = (MeasureType)Convert.ToInt32(model["measureType"]);
                    var quantity = (int)model["quantity"];
                    var kindOfBottle = (KindOfProductContainer)Convert.ToInt32(model["kindOfBottle"]);
                    var dosesStr = ((object)model["doses"])?.ToString();
                    var doses = !string.IsNullOrWhiteSpace(dosesStr) ? JsonConvert.DeserializeObject<DosesInput[]>(dosesStr) : null;
                    return await db.Products.CreateEditProduct(id, commercialName, idActiveIngredient, brand, doses, measureType, quantity, kindOfBottle);
                }, claims);
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                var managerLocal = await ContainerMethods.AgroManager(claims);
                var resultLocal = await managerLocal.Products.GetProduct(id);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.Products.GetProducts();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/custom_notification_events
        [FunctionName("CustomNotificationEvents")]
        public static async Task<IActionResult> CustomNotificationEvents([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v2/custom_notification_events/{idSpecie}/{page}/{totalByPage}/{desc?}")] HttpRequest req, string idSpecie, int page, int totalByPage, string desc,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var orderDate = string.IsNullOrWhiteSpace(desc) || desc.ToLower().Equals("desc");
            if (!orderDate && !desc.ToLower().Equals("asc"))
                return new BadRequestResult();
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.CustomManager.MobileEvents.GetNotificationPreOrdersResult(idSpecie, page, totalByPage, orderDate);
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/notification/barrack/{idBarrack}
        [FunctionName("CustomNotificationBarrack")]
        public static async Task<IActionResult> CustomNotificationBarrack([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v2/notification/barrack/{idBarrack}")] HttpRequest req, string idBarrack,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            return ContainerMethods.GetJsonGetContainer(await manager.NotificationEvents.GetEventsByBarrackId(idBarrack), log);
        }
        #endregion

        #region v2/notification/barrack/{idBarrack}/phenological/{idPhenological}
        [FunctionName("CustomNotificationBarrackPhenologicalEvent")]
        public static async Task<IActionResult> CustomNotificationBarrackPhenologicalEvent([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v2/notification/barrack/{idBarrack}/phenological/{idPhenological}")] HttpRequest req, string idBarrack, string idPhenological, ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            return ContainerMethods.GetJsonGetContainer(await manager.NotificationEvents.GetEventsByBarrackPhenologicalEventId(idBarrack, idPhenological), log);
        }
        #endregion

        #region v2/notification_events
        [FunctionName("NotificationEvents")]
        public static async Task<IActionResult> NotificationEvents([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "v2/notification_events/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var newModel = model["_parts"][0][1];
                    var idPhenologicalEvent = (string)newModel["idPhenologicalEvent"];
                    var description = (string)newModel["description"];
                    var base64 = (string)newModel["base64"];
                    var barrack = (string)newModel["idBarrack"];
                    var response = await db.NotificationEvents.SaveNewNotificationEvent(barrack, idPhenologicalEvent, base64, description);
                    var evt = await db.NotificationEvents.GetEvent(response.IdRelated);
                    var url = evt.Result.PicturePath;
                    await email.SendEmail("Notificacion", 
                        $@"<html>
                            <body>
                                <p> Estimado(a), </p>
                                <p> Llego una notificacion </p>
                                <img src='{url}' style='width:50%;height:auto;'>
                                <p> Atentamente,<br> -Aresa </br></p>
                            </body>
                        </html>");
                    return response;
                },claims);
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                var managerLocal = await ContainerMethods.AgroManager(claims);
                if (id.Equals("init"))
                {
                    var resultEvent = await managerLocal.CustomManager.MobileEvents.GetEventData();
                    return ContainerMethods.GetJsonGetContainer(resultEvent, log);
                }
                if (id.Equals("ts"))
                {
                    var resultTs = await managerLocal.CustomManager.MobileEvents.GetMobileEventTimestamp();
                    return ContainerMethods.GetJsonGetContainer(resultTs, log);
                }
                var resultLocal = await managerLocal.NotificationEvents.GetEvent(id);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.NotificationEvents.GetEvents();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/orders
        [FunctionName("Orders")]
        public static async Task<IActionResult> Orders([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/orders/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations<string>(req.Body, log, async (db, model) =>
                {
                    try
                    {
                        var input = JsonConvert.DeserializeObject<ApplicationOrderInput>(model.ToString());
                        return await db.ApplicationOrders.SaveNewApplicationOrder(input);
                    }
                    catch (Exception E)
                    {
                        throw E;
                    }
                },claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                if (string.IsNullOrWhiteSpace(id))
                    return new NotFoundResult();
                return await ContainerMethods.ApiPostOperations<OutPutApplicationOrder>(req.Body, log, async (db, model) =>{
                        var input = JsonConvert.DeserializeObject<ApplicationOrderInput>(model.ToString());
                        return await db.ApplicationOrders.SaveEditApplicationOrder(id, input);
                    }, claims);
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                var managerLocal = await ContainerMethods.AgroManager(claims);
                var resultLocal = await managerLocal.ApplicationOrders.GetApplicationOrder(id);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.ApplicationOrders.GetApplicationOrders();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }

        #endregion

        #region v2/sectors
        [FunctionName("SectorV2")]
        public static async Task<IActionResult> Sector([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/sectors/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    return await db.Sectors.SaveNewSector(name);
                },claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    return await db.Sectors.SaveEditSector(id, name);
                },claims);
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                var managerLocal = await ContainerMethods.AgroManager(claims);
                var resultLocal = await managerLocal.Sectors.GetSector(id);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.Sectors.GetSectors();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/plotlands
        [FunctionName("PlotLandsV2")]
        public static async Task<IActionResult> PlotLandsV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/plotlands/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var idSector = (string)model["idSector"];
                    return await db.PlotLands.SaveNewPlotLand(name, idSector);
                }, claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var idSector = (string)model["idSector"];
                    return await db.PlotLands.SaveEditPlotLand(id, name, idSector);
                }, claims);
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                var managerLocal = await ContainerMethods.AgroManager(claims);
                var resultLocal = await managerLocal.PlotLands.GetPlotLand(id);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.PlotLands.GetPlotLands();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/varieties
        [FunctionName("VarietiesV2")]
        public static async Task<IActionResult> VarietiesV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/varieties/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {

                    var name = (string)model["name"];
                    var idSpecie = (string)model["idSpecie"];
                    var abbreviation = (string)model["abbreviation"];
                    return await db.Varieties.SaveNewVariety(name, abbreviation, idSpecie);
                },claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var idSpecie = (string)model["idSpecie"];
                    var abbreviation = (string)model["abbreviation"];
                    return await db.Varieties.SaveEditVariety(id, name, abbreviation, idSpecie);
                },claims);
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                var managerLocal = await ContainerMethods.AgroManager(claims);
                var resultLocal = await managerLocal.Varieties.GetVariety(id);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.Varieties.GetVarieties();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/barracks
        [FunctionName("BarracksV2")]
        public static async Task<IActionResult> BarracksV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/barracks/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
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
                    var idRootstock = (string)model["idRootstock"];
                    return await db.Barracks.SaveNewBarrack(name, idPlotland, hectares, plantingYear, idVariety, numberOfPlants, idPollinator, idRootstock);
                },claims);
            }
            if (req.Method.ToLower().Equals("put"))
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
                    var idRootstock = (string)model["idRootstock"];
                    return await db.Barracks.SaveEditBarrack(id, name, idPlotland, hectares, plantingYear, idVariety, numberOfPlants, idPollinator, idRootstock);
                },claims);
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                var managerLocal = await ContainerMethods.AgroManager(claims);
                var resultLocal = await managerLocal.Barracks.GetBarrack(id);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);

            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.Barracks.GetBarracks();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/phenological_preorders
        [FunctionName("PhenologicalPreOrders")]
        public static async Task<IActionResult> PhenologicalPreOrders([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/phenological_preorders/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var idFolder = (string)model["idOrderFolder"];
                    var arr = (string)model["idBarracks"].ToString();
                    var idBarracks = JsonConvert.DeserializeObject<string[]>(arr);

                    return await db.PhenologicalPreOrders.SaveNewPhenologicalPreOrder(name, idFolder, idBarracks.ToList());
                }, claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var idFolder = (string)model["idOrderFolder"];
                    var arr = (string)model["idBarracks"].ToString();
                    var idBarracks = JsonConvert.DeserializeObject<string[]>(arr);
                    return await db.PhenologicalPreOrders.SaveEditPhenologicalPreOrder(id, name, idFolder, idBarracks.ToList());
                }, claims);
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                var managerLocal = await ContainerMethods.AgroManager(claims);
                var resultLocal = await managerLocal.PhenologicalPreOrders.GetPhenologicalPreOrder(id);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.PhenologicalPreOrders.GetPhenologicalPreOrders();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/roles
        [FunctionName("Roles")]
        public static async Task<IActionResult> Roles([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/roles/{id?}")] HttpRequest req, ILogger log, string id)
        {
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    return await db.Roles.SaveNewRole(name);
                }, claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    return await db.Roles.SaveEditRole(id, name);
                }, claims);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.Roles.GetRoles();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/jobs
        [FunctionName("Jobs")]
        public static async Task<IActionResult> Jobs([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/jobs/{id?}")] HttpRequest req, ILogger log, string id)
        {
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var isApplicator = (bool)model["isApplicator"];
                    return await db.Jobs.SaveNewJob(name, isApplicator);
                }, claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var isApplicator = (bool)model["isApplicator"];
                    return await db.Jobs.SaveEditJob(id, name, isApplicator);
                }, claims);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.Jobs.GetJobs();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/users
        [FunctionName("Users")]
        public static async Task<IActionResult> Users([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/users/{id?}")] HttpRequest req, string id, ILogger log)
        {
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            if (req.Method.ToLower().Equals("post"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var rut = (string)model["rut"];
                    var email = (string)model["email"];
                    var idJob = (string)model["idJob"];
                    var idsRoles = (string[])model["idsRoles"].ToObject<string[]>();
                    return await db.Users.SaveNewUser(name, rut, email, idJob, idsRoles);
                }, claims);
            }
            if (req.Method.ToLower().Equals("put"))
            {
                return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                {
                    var name = (string)model["name"];
                    var rut = (string)model["rut"];
                    var email = (string)model["email"];
                    var idJob = (string)model["idJob"];
                    var idsRoles = (string[])model["idsRoles"].ToObject<string[]>();
                    return await db.Users.SaveEditUser(id, name, rut, email, idJob, idsRoles);
                }, claims);
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                var managerLocal = await ContainerMethods.AgroManager(claims);
                var resultLocal = await managerLocal.Users.GetUser(id);
                return ContainerMethods.GetJsonGetContainer(resultLocal, log);
            }
            var manager = await ContainerMethods.AgroManager(claims);
            var result = await manager.Users.GetUsers();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

    }
}
