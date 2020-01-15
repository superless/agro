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
using trifenix.agro.model.external;
using System.Collections.Generic;
using System.IO;
using trifenix.agro.db.model.agro.orders;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace trifenix.agro.functions {
    public static class MainAgroFunction {

        #region v2/phenological_events
        [FunctionName("PhenologicalEventV2")]
        public static async Task<IActionResult> PhenologicalEventV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/phenological_events/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<List<PhenologicalEvent>> result = null;
            switch (req.Method.ToLower()){
                case "get":
                    result = await manager.PhenologicalEvents.GetPhenologicalEvents();
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var initDate = (DateTime)model["startDate"];
                        var endDate = (DateTime)model["endDate"];
                        return await db.PhenologicalEvents.SaveNewPhenologicalEvent(name, initDate, endDate);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var initDate = (DateTime)model["startDate"];
                        var endDate = (DateTime)model["endDate"];
                        return await db.PhenologicalEvents.SaveEditPhenologicalEvent(id, name, initDate, endDate);
                    }, claims);
            }
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/seasons
        [FunctionName("SeasonV2")]
        public static async Task<IActionResult> SeasonV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/seasons/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<List<Season>> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    result = await manager.Seasons.GetSeasons();
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var initDate = (DateTime)model["startDate"];
                        var endDate = (DateTime)model["endDate"];
                        return await db.Seasons.SaveNewSeason(initDate, endDate);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var initDate = (DateTime)model["startDate"];
                        var endDate = (DateTime)model["endDate"];
                        var current = (bool)model["current"];
                        return await db.Seasons.SaveEditSeason(id, initDate, endDate, current);
                    }, claims);
            }
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/specie
        [FunctionName("SpecieV2")]
        public static async Task<IActionResult> SpecieV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/species/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<List<Specie>> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    result = await manager.Species.GetSpecies();
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var abbreviation = (string)model["abbreviation"];
                        return await db.Species.SaveNewSpecie(name, abbreviation);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var abbreviation = (string)model["abbreviation"];
                        return await db.Species.SaveEditSpecie(id, name, abbreviation);
                    }, claims);
            }
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/rootstock
        [FunctionName("RootstockV2")]
        public static async Task<IActionResult> RootstockV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/rootstock/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<List<Rootstock>> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    result = await manager.Rootstock.GetRootstocks();
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var abbreviation = (string)model["abbreviation"];
                        return await db.Rootstock.SaveNewRootstock(name, abbreviation);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var abbreviation = (string)model["abbreviation"];
                        return await db.Rootstock.SaveEditRootstock(id, name, abbreviation);
                    }, claims);
            }
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/certified_entities
        [FunctionName("CertifiedEntity")]
        public static async Task<IActionResult> CertifiedEntity([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/certified_entities/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<CertifiedEntity> result = null;
            switch (req.Method.ToLower()){
                case "get":
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        result = await manager.CertifiedEntities.GetCertifiedEntity(id);
                        return ContainerMethods.GetJsonGetContainer(result, log);
                    }
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var abbreviation = (string)model["abbreviation"];
                        return await db.CertifiedEntities.SaveNewCertifiedEntity(name, abbreviation);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var abbreviation = (string)model["abbreviation"];
                        return await db.CertifiedEntities.SaveEditCertifiedEntity(id, name, abbreviation);
                    }, claims);
            }
            ExtGetContainer<List<CertifiedEntity>> resultGetAll = await manager.CertifiedEntities.GetCertifiedEntities();
            return ContainerMethods.GetJsonGetContainer(resultGetAll, log);
        }
        #endregion

        #region v2/ingredient_categories
        [FunctionName("CategoryIngredientsV2")]
        public static async Task<IActionResult> CategoryIngredientsV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/ingredient_categories/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<List<IngredientCategory>> result = null;
            switch (req.Method.ToLower()){
                case "get":
                    result = await manager.IngredientCategories.GetIngredientCategories();
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        return await db.IngredientCategories.SaveNewIngredientCategory(name);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        return await db.IngredientCategories.SaveEditIngredientCategory(id, name);
                    }, claims);
            }
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/ingredients
        [FunctionName("IngredientsV2")]
        public static async Task<IActionResult> IngredientsV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/ingredients/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<List<Ingredient>> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    result = await manager.Ingredients.GetIngredients();
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var idCategory = (string)model["idCategory"];
                        return await db.Ingredients.SaveNewIngredient(name, idCategory);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var idCategory = (string)model["idCategory"];
                        return await db.Ingredients.SaveEditIngredient(id, name, idCategory);
                    }, claims);
            }
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/targets
        [FunctionName("TargetV2")]
        public static async Task<IActionResult> TargetV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/targets/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<List<ApplicationTarget>> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    result = await manager.ApplicationTargets.GetAplicationsTarget();
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        return await db.ApplicationTargets.SaveNewApplicationTarget(name);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        return await db.ApplicationTargets.SaveEditApplicationTarget(id, name);
                    }, claims);
            }
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/order_folders
        [FunctionName("OrderFolder")]
        public static async Task<IActionResult> OrderFolder([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/order_folders/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<OrderFolder> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        result = await manager.OrderFolder.GetOrderFolder(id);
                        return ContainerMethods.GetJsonGetContainer(result, log);
                    }
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var idPhenologicalEvent = (string)model["idPhenologicalEvent"];
                        var idApplicationTarget = (string)model["idApplicationTarget"];
                        var categoryId = (string)model["idCategory"];
                        var idSpecie = (string)model["idSpecie"];
                        var idIngredient = (string)model["idIngredient"];
                        return await db.OrderFolder.SaveNewOrderFolder(idPhenologicalEvent, idApplicationTarget, categoryId, idSpecie, idIngredient);
                    }, claims);
                case "put":
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
            ExtGetContainer<List<OrderFolder>> resultGetAll = await manager.OrderFolder.GetOrderFolders();
            return ContainerMethods.GetJsonGetContainer(resultGetAll, log);
        }
        #endregion

        #region v2/products
        [FunctionName("Product")]
        public static async Task<IActionResult> Product([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/products/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<Product> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        result = await manager.Products.GetProduct(id);
                        return ContainerMethods.GetJsonGetContainer(result, log);
                    }
                    break;
                case "post":
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
                case "put":
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
            ExtGetContainer<List<Product>> resultGetAll = await manager.Products.GetProducts();
            return ContainerMethods.GetJsonGetContainer(resultGetAll, log);
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
            //ClaimsPrincipal claims = await Auth.Validate(req);
            //if (claims == null)
            //    return new UnauthorizedResult();
            //var manager = await ContainerMethods.AgroManager(claims);
            //Email email = new Email(manager.Users.GetUsers().Result.Result);

            HttpClient client = new HttpClient();
            var authorizationHeader = req.Headers?["Authorization"];
            string[] parts = authorizationHeader?.ToString().Split(null) ?? new string[0];
            string accessToken = string.Empty;
            if (parts.Length == 2 && parts[0].Equals("Bearer"))
                accessToken =  parts[1];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic body = JsonConvert.DeserializeObject(requestBody);
            var newBody = body["_parts"][0][1];
            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(newBody));
            MemoryStream stream = new MemoryStream(byteArray);
            var inputData = new StreamContent(stream);
            string ipNgrok = Environment.GetEnvironmentVariable("ipNgrok", EnvironmentVariableTarget.Process);
            await client.PostAsync("https://" + ipNgrok + ".ngrok.io/api/v2/debugroute", inputData);
            client.Dispose();
            return null;

            //ExtGetContainer<NotificationEvent> result = null;
            //switch (req.Method.ToLower()) {
            //    case "get":
            //        if (!string.IsNullOrWhiteSpace(id)) {
            //            switch (id) {
            //                case "init":
            //                    var resultEvent = await manager.CustomManager.MobileEvents.GetEventData();
            //                    return ContainerMethods.GetJsonGetContainer(resultEvent, log);
            //                case "ts":
            //                    var resultTs = await manager.CustomManager.MobileEvents.GetMobileEventTimestamp();
            //                    return ContainerMethods.GetJsonGetContainer(resultTs, log);
            //                default:
            //                    result = await manager.NotificationEvents.GetEvent(id);
            //                    return ContainerMethods.GetJsonGetContainer(result, log);
            //            }
            //        }
            //        break;
            //    case "post":
            //        return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) => {
            //            var newModel = model["_parts"][0][1];
            //            var idPhenologicalEvent = (string)newModel["idPhenologicalEvent"];
            //            var description = (string)newModel["description"];
            //            var base64 = (string)newModel["base64"];
            //            var barrack = (string)newModel["idBarrack"];
            //            //var lat = (float)newModel["latitude"];
            //            //var lon = (float)newModel["longitude"];
            //            var response = await db.NotificationEvents.SaveNewNotificationEvent(barrack, idPhenologicalEvent, base64, description, 0F, 0F);
            //            var evt = await db.NotificationEvents.GetEvent(response.IdRelated);
            //            var url = evt.Result.PicturePath;
            //            email.SendEmail("Notificacion",
            //                $@"<html>
            //                <body>
            //                    <p> Estimado(a), </p>
            //                    <p> Llego una notificacion </p>
            //                    <img src='{url}' style='width:50%;height:auto;'>
            //                    <p> Atentamente,<br> -Aresa </br></p>
            //                </body>
            //            </html>");
            //            return response;
            //        }, claims);
            //}
            //ExtGetContainer<List<NotificationEvent>> resultGetAll = await manager.NotificationEvents.GetEvents();
            //return ContainerMethods.GetJsonGetContainer(resultGetAll, log);
        }
        #endregion



        [FunctionName("OrderFilter")]
        public static async Task<IActionResult> OrderFilter([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v2/orders/search/{search}/{page}/{quantity}/{order}")] HttpRequest req, string search, int page, int quantity, string order, ILogger log)
        {
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            var orderDate = string.IsNullOrWhiteSpace(order) || order.ToLower().Equals("desc");
            var result = await manager.ApplicationOrders.GetApplicationOrdersByPage(search, page, quantity, orderDate);
            return ContainerMethods.GetJsonGetContainer(result, log);
        }

        [FunctionName("OrderSimpleFilter")]
        public static async Task<IActionResult> OrderSimpleFilter([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v2/orders/search/simple/{search}/{page}/{quantity}/{order}")] HttpRequest req, string search, int page, int quantity, string order, ILogger log)
        {
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            var orderDate = string.IsNullOrWhiteSpace(order) || order.ToLower().Equals("desc");
            var result = manager.ApplicationOrders.GetOrderSearch(search, page, quantity, orderDate);
            return ContainerMethods.GetJsonGetContainer(result, log);
        }


        #region v2/orders
        [FunctionName("Orders")]
        public static async Task<IActionResult> Orders([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/orders/{id?}/{totalByPage?}/{desc?}")] HttpRequest req, string id, int? totalByPage, string desc, ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<OutPutApplicationOrder> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        if (int.TryParse(id, out var resultInt)) break;
                        result = await manager.ApplicationOrders.GetApplicationOrder(id);
                        return ContainerMethods.GetJsonGetContainer(result, log);
                    }
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations<string>(req.Body, log, async (db, model) =>
                    {
                        var input = JsonConvert.DeserializeObject<ApplicationOrderInput>(model.ToString());
                        return await db.ApplicationOrders.SaveNewApplicationOrder(input);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations<OutPutApplicationOrder>(req.Body, log, async (db, model) => {
                        var input = JsonConvert.DeserializeObject<ApplicationOrderInput>(model.ToString());
                        return await db.ApplicationOrders.SaveEditApplicationOrder(id, input);
                    }, claims);
            }
            if (!string.IsNullOrWhiteSpace(id) && totalByPage.HasValue)
            {
                var isPage = int.TryParse(id, out var page);

                if (!isPage) return new BadRequestResult();
                var orderDate = string.IsNullOrWhiteSpace(desc) || desc.ToLower().Equals("desc");
                if (!orderDate && !desc.ToLower().Equals("asc"))
                    return new BadRequestResult();
                var resultGetByPageAll = await manager.ApplicationOrders.GetApplicationOrdersByPage(page, totalByPage??10, orderDate);
                return ContainerMethods.GetJsonGetContainer(resultGetByPageAll, log);
            }

            ExtGetContainer<List<OutPutApplicationOrder>> resultGetAll = await manager.ApplicationOrders.GetApplicationOrders();
            return ContainerMethods.GetJsonGetContainer(resultGetAll, log);
        }
        #endregion

        #region v2/ordersByExecutionStatus
        [FunctionName("OrderByExecutionStatus")]
        public static async Task<IActionResult> OrderByExecutionStatus([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v2/orders/getByExecutionStatus/{status}")] HttpRequest req, ExecutionStatus status, ILogger log) {
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<List<ExecutionOrder>> resultGetByStatus = await manager.ExecutionOrders.GetExecutionOrders();
            resultGetByStatus.Result = resultGetByStatus.Result.Where(execution => execution.ExecutionStatus == status).ToList();
            ExtGetContainer<List<OutPutApplicationOrder>> resultGetAll = await manager.ApplicationOrders.GetApplicationOrders();
            resultGetAll.Result = resultGetAll.Result.Where(order => resultGetByStatus.Result.Any(execution => execution.Order.Id.Equals(order.Id))).ToList();
            return ContainerMethods.GetJsonGetContainer(resultGetAll, log);
        }
        #endregion

        #region v2/sectors
        [FunctionName("SectorV2")]
        public static async Task<IActionResult> Sector([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/sectors/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<Sector> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        result = await manager.Sectors.GetSector(id);
                        return ContainerMethods.GetJsonGetContainer(result, log);
                    }
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        return await db.Sectors.SaveNewSector(name);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        return await db.Sectors.SaveEditSector(id, name);
                    }, claims);
            }
            ExtGetContainer<List<Sector>> resultGetAll = await manager.Sectors.GetSectors();
            return ContainerMethods.GetJsonGetContainer(resultGetAll, log);
        }
        #endregion

        #region v2/plotlands
        [FunctionName("PlotLandsV2")]
        public static async Task<IActionResult> PlotLandsV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/plotlands/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<PlotLand> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        result = await manager.PlotLands.GetPlotLand(id);
                        return ContainerMethods.GetJsonGetContainer(result, log);
                    }
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var idSector = (string)model["idSector"];
                        return await db.PlotLands.SaveNewPlotLand(name, idSector);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var idSector = (string)model["idSector"];
                        return await db.PlotLands.SaveEditPlotLand(id, name, idSector);
                    }, claims);
            }
            ExtGetContainer<List<PlotLand>> resultGetAll = await manager.PlotLands.GetPlotLands();
            return ContainerMethods.GetJsonGetContainer(resultGetAll, log);
        }
        #endregion

        #region v2/varieties
        [FunctionName("VarietiesV2")]
        public static async Task<IActionResult> VarietiesV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/varieties/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<Variety> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        result = await manager.Varieties.GetVariety(id);
                        return ContainerMethods.GetJsonGetContainer(result, log);
                    }
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var idSpecie = (string)model["idSpecie"];
                        var abbreviation = (string)model["abbreviation"];
                        return await db.Varieties.SaveNewVariety(name, abbreviation, idSpecie);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var idSpecie = (string)model["idSpecie"];
                        var abbreviation = (string)model["abbreviation"];
                        return await db.Varieties.SaveEditVariety(id, name, abbreviation, idSpecie);
                    }, claims);
            }
            ExtGetContainer<List<Variety>> resultGetAll = await manager.Varieties.GetVarieties();
            return ContainerMethods.GetJsonGetContainer(resultGetAll, log);
        }
        #endregion

        #region v2/barracks
        [FunctionName("BarracksV2")]
        public static async Task<IActionResult> BarracksV2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/barracks/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<Barrack> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        result = await manager.Barracks.GetBarrack(id);
                        return ContainerMethods.GetJsonGetContainer(result, log);
                    }
                    break;
                case "post":
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
                    }, claims);
                case "put":
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
                    }, claims);
            }
            ExtGetContainer<List<Barrack>> resultGetAll = await manager.Barracks.GetBarracks();
            return ContainerMethods.GetJsonGetContainer(resultGetAll, log);
        }
        #endregion

        #region v2/phenological_preorders
        [FunctionName("PhenologicalPreOrders")]
        public static async Task<IActionResult> PhenologicalPreOrders([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/phenological_preorders/{id?}")] HttpRequest req, string id,ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<PhenologicalPreOrder> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        result = await manager.PhenologicalPreOrders.GetPhenologicalPreOrder(id);
                        return ContainerMethods.GetJsonGetContainer(result, log);
                    }
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var idFolder = (string)model["idOrderFolder"];
                        var arr = (string)model["idBarracks"].ToString();
                        var idBarracks = JsonConvert.DeserializeObject<string[]>(arr);

                        return await db.PhenologicalPreOrders.SaveNewPhenologicalPreOrder(name, idFolder, idBarracks.ToList());
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        var idFolder = (string)model["idOrderFolder"];
                        var arr = (string)model["idBarracks"].ToString();
                        var idBarracks = JsonConvert.DeserializeObject<string[]>(arr);
                        return await db.PhenologicalPreOrders.SaveEditPhenologicalPreOrder(id, name, idFolder, idBarracks.ToList());
                    }, claims);
            }
            ExtGetContainer<List<PhenologicalPreOrder>> resultGetAll = await manager.PhenologicalPreOrders.GetPhenologicalPreOrders();
            return ContainerMethods.GetJsonGetContainer(resultGetAll, log);
        }
        #endregion

        #region v2/roles
        [FunctionName("Roles")]
        public static async Task<IActionResult> Roles([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/roles/{id?}")] HttpRequest req, ILogger log, string id){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<List<Role>> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    result = await manager.Roles.GetRoles();
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        return await db.Roles.SaveNewRole(name);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        return await db.Roles.SaveEditRole(id, name);
                    }, claims);
            }
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/jobs
        [FunctionName("Jobs")]
        public static async Task<IActionResult> Jobs([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/jobs/{id?}")] HttpRequest req, ILogger log, string id){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<List<Job>> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    result = await manager.Jobs.GetJobs();
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        return await db.Jobs.SaveNewJob(name);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var name = (string)model["name"];
                        return await db.Jobs.SaveEditJob(id, name);
                    }, claims);
            }
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/users
        [FunctionName("Users")]
        public static async Task<IActionResult> Users([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/users/{id?}")] HttpRequest req, string id, ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<UserApplicator> result = null;
            switch (req.Method.ToLower()) {
                case "get":
                    if (!string.IsNullOrWhiteSpace(id)) {
                        result = await manager.Users.GetUser(id);
                        return ContainerMethods.GetJsonGetContainer(result, log);
                    }
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) => {
                        var name = (string)model?["name"];
                        var rut = (string)model?["rut"];
                        var email = (string)model?["email"];
                        var idJob = (string)model?["idJob"];
                        var idsRoles = (string[])model?["idsRoles"]?.ToObject<string[]>();
                        var idTractor = (string)model?["idTractor"];
                        var idNebulizer = (string)model?["idNebulizer"];
                        return await db.Users.SaveNewUser(name, rut, email, idJob, idsRoles, idNebulizer, idTractor);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) => {
                        var name = (string)model["name"];
                        var rut = (string)model["rut"];
                        var email = (string)model["email"];
                        var idJob = (string)model["idJob"];
                        var idsRoles = (string[])model["idsRoles"].ToObject<string[]>();
                        var idTractor = (string)model["idTractor"];
                        var idNebulizer = (string)model["idNebulizer"];
                        return await db.Users.SaveEditUser(id, name, rut, email, idJob, idsRoles, idNebulizer, idTractor);
                    }, claims);
            }
            ExtGetContainer<List<UserApplicator>> resultGetAll = await manager.Users.GetUsers();
            return ContainerMethods.GetJsonGetContainer(resultGetAll, log);
        }
        #endregion

        #region v2/usersByRole
        [FunctionName("UserByRole")]
        public static async Task<IActionResult> UserByRole([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v2/users/getByRole/{roleName}")] HttpRequest req, string roleName, ILogger log) {
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<List<UserApplicator>> resultGetAllApplicator = await manager.Users.GetUsers();
            resultGetAllApplicator.Result = resultGetAllApplicator.Result.Where(user => user.Roles.Any(role => role.Name.Equals(roleName))).ToList();
            return ContainerMethods.GetJsonGetContainer(resultGetAllApplicator, log);
        }
        #endregion

        #region v2/userInfo
        [FunctionName("UserInfo")]
        public static async Task<IActionResult> UsersRoles([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v2/userinfo")] HttpRequest req, ILogger log){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<UserApplicator> result = await manager.Users.GetUserFromToken();
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/nebulizers
        [FunctionName("Nebulizers")]
        public static async Task<IActionResult> Nebulizers([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/nebulizers/{id?}")] HttpRequest req, ILogger log, string id){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<List<Nebulizer>> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    result = await manager.Nebulizers.GetNebulizers();
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var brand = (string)model["brand"];
                        var code = (string)model["code"];
                        return await db.Nebulizers.SaveNewNebulizer(brand, code);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var brand = (string)model["brand"];
                        var code = (string)model["code"];
                        return await db.Nebulizers.SaveEditNebulizer(id, brand, code);
                    }, claims);
            }
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/tractors
        [FunctionName("Tractors")]
        public static async Task<IActionResult> Tractors([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/tractors/{id?}")] HttpRequest req, ILogger log, string id){
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<List<Tractor>> result = null;
            switch (req.Method.ToLower())
            {
                case "get":
                    result = await manager.Tractors.GetTractors();
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var brand = (string)model["brand"];
                        var code = (string)model["code"];
                        return await db.Tractors.SaveNewTractor(brand, code);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) =>
                    {
                        var brand = (string)model["brand"];
                        var code = (string)model["code"];
                        return await db.Tractors.SaveEditTractor(id, brand, code);
                    }, claims);
            }
            return ContainerMethods.GetJsonGetContainer(result, log);
        }
        #endregion

        #region v2/executions
        [FunctionName("Executions")]
        public static async Task<IActionResult> Executions([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/executions/{id?}")] HttpRequest req, string id, ILogger log) {
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<ExecutionOrder> result = null;
            switch (req.Method.ToLower()) {
                case "get":
                    if (!string.IsNullOrWhiteSpace(id)) {
                        result = await manager.ExecutionOrders.GetExecutionOrder(id);
                        return ContainerMethods.GetJsonGetContainer(result, log);
                    }
                    break;
                case "post":
                    return await ContainerMethods.ApiPostOperations<string>(req.Body, log, async (db, model) => {
                        string idOrder = (string)model["idOrder"];
                        string idUserApplicator = (string)model["idUserApplicator"];
                        string idNebulizer = (string)model["idNebulizer"];
                        string idProduct = (string)model["idProduct"];
                        double quantityByHectare = (double)model["quantityByHectare"];
                        string idTractor = (string)model["idTractor"];
                        string commentary = (string)model["commentary"];
                        return await db.ExecutionOrders.SaveNewExecutionOrder(idOrder, idUserApplicator, idNebulizer, idProduct, quantityByHectare, idTractor, commentary);
                    }, claims);
                case "put":
                    return await ContainerMethods.ApiPostOperations<ExecutionOrder>(req.Body, log, async (db, model) => {
                        string idOrder = (string)model["idOrder"];
                        string idUserApplicator = (string)model["idUserApplicator"];
                        string idNebulizer = (string)model["idNebulizer"];
                        string idProduct = (string)model["idProduct"];
                        double quantityByHectare = (double)model["quantityByHectare"];
                        string idTractor = (string)model["idTractor"];
                        return await db.ExecutionOrders.SaveEditExecutionOrder(id, idOrder, idUserApplicator, idNebulizer, idProduct, quantityByHectare, idTractor);
                    }, claims);
            }
            ExtGetContainer<List<ExecutionOrder>> resultGetAll = await manager.ExecutionOrders.GetExecutionOrders();
            return ContainerMethods.GetJsonGetContainer(resultGetAll, log);
        }
        #endregion

        #region v2/executionsByStatus
        [FunctionName("ExecutionByStatus")]
        public static async Task<IActionResult> ExecutionByStatus([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v2/executions/getByStatus/{status}")] HttpRequest req, ExecutionStatus status, ILogger log) {
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            ExtGetContainer<List<ExecutionOrder>> resultGetByStatus = await manager.ExecutionOrders.GetExecutionOrders();
            resultGetByStatus.Result = resultGetByStatus.Result.Where(execution => execution.ExecutionStatus == status).ToList();
            return ContainerMethods.GetJsonGetContainer(resultGetByStatus, log);
        }
        #endregion

        #region v2/Executions_ChangeStatus
        [FunctionName("Execution_ChangeStatus")]
        public static async Task<IActionResult> Execution_ChangeStatus([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v2/executions/changeStatus/{idExecution}")] HttpRequest req, string idExecution, ILogger log) {
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            return await ContainerMethods.ApiPostOperations(req.Body, log, async (db, model) => {
                var type = (string)model["type"];
                var value = (int)model["value"];
                var commentary = (string)model["commentary"];
                return await manager.ExecutionOrders.SetStatus(idExecution, type, value, commentary);
            }, claims);
        }
        #endregion

        #region v2/executionsAddCommentary
        [FunctionName("ExecutionsAddCommentary")]
        public static async Task<IActionResult> ExecutionsAddCommentary([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v2/executions/add_commentary/{id}")] HttpRequest req, string id, ILogger log) {
            ClaimsPrincipal claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            var manager = await ContainerMethods.AgroManager(claims);
            return await ContainerMethods.ApiPostOperations<ExecutionOrder>(req.Body, log, async (db, model) => {
                string commentary = (string)model["commentary"];
                return await db.ExecutionOrders.AddCommentaryToExecutionOrder(id, commentary);
            }, claims);
        }
        #endregion

        [FunctionName("DebugRoute")]
        public static async Task<IActionResult> DebugRoute([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "v2/debugroutes/{id?}")] HttpRequest req, ILogger log, string id){
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic result = JsonConvert.DeserializeObject(requestBody);
            return result;
        }

    }
}