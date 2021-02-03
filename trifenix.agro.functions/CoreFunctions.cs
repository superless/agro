
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using trifenix.agro.external.operations.helper;
using trifenix.agro.functions.Helper;
using trifenix.agro.functions.mantainers;
using trifenix.agro.functions.settings;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using trifenix.connect.agro.external.helper;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.queries;
using trifenix.connect.agro_model_input;
using trifenix.connect.aad.auth;
using trifenix.connect.bus;
using trifenix.connect.db.cosmos.exceptions;
using trifenix.connect.interfaces.auth;
using trifenix.connect.mdm.containers;
using AzureFunctions.Extensions.Swashbuckle.Attribute;

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
        [SwaggerIgnore]
        [FunctionName("Login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic credenciales = JsonConvert.DeserializeObject(requestBody);
            string clientId = Environment.GetEnvironmentVariable("clientID", EnvironmentVariableTarget.Process);
            string scope = Environment.GetEnvironmentVariable("scope", EnvironmentVariableTarget.Process);
            string clientSecret = Environment.GetEnvironmentVariable("clientSecret", EnvironmentVariableTarget.Process);
            string username = (string)credenciales["username"];
            string password = (string)credenciales["password"];
            string grantType = "password";
            string tenant = Environment.GetEnvironmentVariable("tenant", EnvironmentVariableTarget.Process);
            string endPoint = $"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token";
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

        [SwaggerIgnore]
        [FunctionName("MessagesNegotiateBinding")]
        public static async Task<IActionResult> NegotiatBindingAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "negotiate")] HttpRequest req,
        IBinder binder,
        ILogger log)
        {

            string AUTH_HEADER_NAME = "Authorization";
            string BEARER_PREFIX = "Bearer ";
            if (req.Headers.ContainsKey(AUTH_HEADER_NAME) && req.Headers[AUTH_HEADER_NAME].ToString().StartsWith(BEARER_PREFIX))
            {
                var token = req.Headers[AUTH_HEADER_NAME].ToString().Substring(BEARER_PREFIX.Length);

                if (token.Equals("cloud-app"))
                {
                    var conn = binder.Bind<SignalRConnectionInfo>(new SignalRConnectionInfoAttribute { HubName = "agro", UserId = "cloud-app" });

                    return new OkObjectResult(conn);
                }

                log.LogInformation("with binding " + token);
                IAuthentication auth = new Authentication(
                    Environment.GetEnvironmentVariable("clientID", EnvironmentVariableTarget.Process),
                    Environment.GetEnvironmentVariable("tenant", EnvironmentVariableTarget.Process),
                    Environment.GetEnvironmentVariable("tenantID", EnvironmentVariableTarget.Process),
                    Environment.GetEnvironmentVariable("validAudiences", EnvironmentVariableTarget.Process).Split(";")
                );
                var claims = await auth.ValidateAccessToken(token);
                string ObjectIdAAD = claims.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                var queries = new CommonQueries(ConfigManager.GetDbArguments);
                // extract userId from token
                var userId = await queries.GetUserIdFromAAD(ObjectIdAAD);
                var connectionInfo = binder.Bind<SignalRConnectionInfo>(new SignalRConnectionInfoAttribute { HubName = "agro", UserId = userId });
                log.LogInformation("negotiated " + connectionInfo);
                //https://gist.github.com/ErikAndreas/72c94a0c8a9e6e632f44522c41be8ee7
                // connectionInfo contains an access key token with a name identifier claim set to the authenticated user
                return new OkObjectResult(connectionInfo);
            }
            else
            {
                // temporal, para conectar winform sin autenticación
                return new UnauthorizedResult();

            }

        }

        [SwaggerIgnore]
        [FunctionName("ServiceBus")]
        public static async Task Handler(
        [ServiceBusTrigger("colageneration-servicebus", Connection = "ServiceBusConnectionString", IsSessionsEnabled = true)] Message message,
        [SignalR(HubName = "agro")] IAsyncCollector<SignalRMessage> signalRMessages,
        ILogger log)
        {
            var opInstance = ServiceBus.Deserialize(message.Body);
            var ObjectIdAAD = opInstance.Value<string>("ObjectIdAAD");
            var queries = new CommonQueries(ConfigManager.GetDbArguments);
            var EntityName = opInstance.Value<string>("EntityName");
            var agro = await ContainerMethods.AgroManager(ObjectIdAAD);
            var entityType = opInstance["EntityType"].ToObject<Type>();
            var repo = agro.GetOperationByInputType(entityType);
            dynamic element = opInstance["Element"].ToObject(entityType);
            element.Id = opInstance.Value<string>("Id");
            string userId = null;

            try
            {
                var saveReturn = await repo.SaveInput(element);
                if (!string.IsNullOrWhiteSpace(ObjectIdAAD))
                {
                    userId = await queries.GetUserIdFromAAD(ObjectIdAAD);
                    await agro.UserActivity.SaveInput(new UserActivityInput
                    {
                        Action = opInstance.Value<string>("HttpMethod").Equals("post") ? UserActivityAction.CREATE : UserActivityAction.MODIFY,
                        Date = DateTime.Now,
                        EntityName = EntityName,
                        EntityId = saveReturn.IdRelated
                    });
                }

                await signalRMessages.AddAsync(new SignalRMessage { Target = "Success", UserId = userId ?? "cloud-app", Arguments = new object[] { EntityName } });
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                await signalRMessages.AddAsync(new SignalRMessage { Target = "Error", UserId = userId ?? "cloud-app", Arguments = new object[] { ex is Validation_Exception ? ((Validation_Exception)ex).ErrorMessages : (object)new string[] { $"{ex.Message}" }, ex.StackTrace } });
            }
        }

        /// <summary>
        /// Creación de Sector
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("sector_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SectorInput))]
        public static async Task<IActionResult> SectorPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "sectors")]
            [RequestBodyType(typeof(SectorInput), "Sector")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Sector, string.Empty);
            return result.JsonResult;

        }


        /// <summary>
        /// Modificación del Sector
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("sector_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> SectorPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "sectors/{id}")]
            [RequestBodyType(typeof(SectorInput), "Sector")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Sector, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Creación de parcela
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("plotland_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PlotLandInput))]
        public static async Task<IActionResult> PlotLandsPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "plotlands")]
            [RequestBodyType(typeof(PlotLandInput), "PlotLand")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.PlotLand, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de parcela
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("plotland_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PlotLandInput))]
        public static async Task<IActionResult> PlotLandsPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "plotlands/{id}")]
            [RequestBodyType(typeof(PlotLandInput), "PlotLand")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.PlotLand, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Especie
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("specie_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SpecieInput))]
        public static async Task<IActionResult> SpeciesPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "species")]
            [RequestBodyType(typeof(SpecieInput), "Specie")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Specie, string.Empty);
            return result.JsonResult;
        }

        /// <summary>
        /// Modificar Especie
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("specie_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SpecieInput))]
        public static async Task<IActionResult> SpeciesPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "species/{id}")]
            [RequestBodyType(typeof(SpecieInput), "Specie")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Specie, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Marca
        /// </summary>
        /// <returns>
        /// Retorna una marca con su id
        /// </returns>
        [HttpPost]
        [FunctionName("brand_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BrandInput))]
        public static async Task<IActionResult> BrandPost(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "brand")]
          [RequestBodyType(typeof(BrandInput), "Brand")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Brand, string.Empty);
            return result.JsonResult;
        }

        /// <summary>
        /// Modificar Marca
        /// </summary>
        /// <returns>
        /// Retorna una marca con el id
        /// </returns>
        [FunctionName("brand_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BrandInput))]
        public static async Task<IActionResult> BrandPut(
          [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "brand/{id}")]
          [RequestBodyType(typeof(BrandInput), "Brand")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Brand, id);
            return result.JsonResult;
        }

        /// <summary>
        /// Añadir Notificación
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("notification_event_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(NotificationEventInput))]
        public static async Task<IActionResult> NotificationsPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "notification")]
            [RequestBodyType(typeof(NotificationEventInput), "NotificationEvent")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.NotificationEvent, string.Empty);
            return result.JsonResult;
        }

        /// <summary>
        /// Editar Notificación
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("notification_event_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(NotificationEventInput))]
        public static async Task<IActionResult> NotificationPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "notification/{id}")]
            [RequestBodyType(typeof(NotificationEventInput), "NotificationEvent")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.NotificationEvent, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Variedad
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("variety_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(VarietyInput))]
        public static async Task<IActionResult> VarietyPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "varieties")]
            [RequestBodyType(typeof(VarietyInput), "Variety")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Variety, string.Empty);
            return result.JsonResult;
        }

        /// <summary>
        /// Añadir Variedad
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("variety_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(VarietyInput))]
        public static async Task<IActionResult> VarietyPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "varieties/{id}")]
            [RequestBodyType(typeof(VarietyInput), "Variety")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Variety, id);
            return result.JsonResult;
        }

        /// <summary>
        /// Añadir Objetivo de aplicación
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("target_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApplicationTargetInput))]
        public static async Task<IActionResult> TargetPost(

            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "targets")]
            [RequestBodyType(typeof(ApplicationTargetInput), "ApplicationTarget")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.ApplicationTarget, string.Empty);
            return result.JsonResult;
        }

        /// <summary>
        /// Modificar objetivo de aplicación
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("target_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApplicationTargetInput))]
        public static async Task<IActionResult> TargetPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "targets/{id}")]
            [RequestBodyType(typeof(ApplicationTargetInput), "ApplicationTarget")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.ApplicationTarget, id);
            return result.JsonResult;
        }

        /// <summary>
        /// Añadir Evento fenológico
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("phenological_event_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PhenologicalEventInput))]
        public static async Task<IActionResult> PhenologicalEventPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "phenological_events")]
            [RequestBodyType(typeof(PhenologicalEventInput), "PhenologicalEvent")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.PhenologicalEvent, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de evento fenológico
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("phenological_event_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PhenologicalEventInput))]
        public static async Task<IActionResult> PhenologicalEventPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "phenological_events/{id}")]
            [RequestBodyType(typeof(PhenologicalEventInput), "PhenologicalEvent")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.PhenologicalEvent, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Entidad Certificadora
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("certified_entities_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CertifiedEntityInput))]
        public static async Task<IActionResult> CertifiedEntityPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "certified_entities")]
            [RequestBodyType(typeof(CertifiedEntityInput), "CertifiedEntity")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.CertifiedEntity, string.Empty);
            return result.JsonResult;
        }

        /// <summary>
        /// Modificación de Entidad Certificadora
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("certified_entities_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CertifiedEntityInput))]
        public static async Task<IActionResult> CertifiedEntityPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "certified_entities/{id}")]
            [RequestBodyType(typeof(CertifiedEntityInput), "CertifiedEntity")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.CertifiedEntity, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Categoría de ingrediente
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("ingredient_categories_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IngredientCategoryInput))]
        public static async Task<IActionResult> CategoryIngredientPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ingredient_categories")]
            [RequestBodyType(typeof(IngredientCategoryInput), "IngredientCategory")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.IngredientCategory, string.Empty);
            return result.JsonResult;
        }

        /// <summary>
        /// Modificación de Categoría de ingredientes
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("ingredient_categories_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IngredientCategoryInput))]
        public static async Task<IActionResult> CategoryIngredientPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ingredient_categories/{id}")]
            [RequestBodyType(typeof(IngredientCategoryInput), "IngredientCategory")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.IngredientCategory, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Ingredientes
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("ingredients_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IngredientInput))]
        public static async Task<IActionResult> IngredientsPost(

            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ingredients")]
            [RequestBodyType(typeof(IngredientInput), "Ingredient")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Ingredient, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de Ingrediente
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("ingredients_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IngredientInput))]
        public static async Task<IActionResult> IngredientPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ingredients/{id}")]
            [RequestBodyType(typeof(IngredientInput), "Ingredient")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Ingredient, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Producto
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("products_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ProductInput))]
        public async static Task<IActionResult> ProductsPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")]
            [RequestBodyType(typeof(ProductInput), "Product")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Product, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de Productos
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("products_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ProductInput))]
        public static async Task<IActionResult> ProductPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "products/{id}")]
            [RequestBodyType(typeof(ProductInput), "Product")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Product, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Rol
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("roles_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RoleInput))]
        public static async Task<IActionResult> RolePost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "roles")]
            [RequestBodyType(typeof(RoleInput), "Roles")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Role, string.Empty);
            return result.JsonResult;
        }



        /// <summary>
        /// Modificación de Rol
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("roles_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RoleInput))]
        public static async Task<IActionResult> RolePut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "roles/{id}")]
            [RequestBodyType(typeof(RoleInput), "Roles")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Role, id);
            return result.JsonResult;
        }



        /// <summary>
        /// Añadir Trabajo
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("jobs_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(JobInput))]
        public static async Task<IActionResult> JobPost(

            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "jobs")]
            [RequestBodyType(typeof(JobInput), "Job")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Job, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de Trabajo
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("jobs_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(JobInput))]
        public static async Task<IActionResult> JobPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "jobs/{id}")]
            [RequestBodyType(typeof(JobInput), "Job")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Job, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Usuario
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("users_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> UserPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.UserApplicator, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de Usuario
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("users_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> UsersPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "users/{id}")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.UserApplicator, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Nebulizador
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("nebulizers_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(NebulizerInput))]
        public static async Task<IActionResult> NebulizersPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "nebulizers")]
            [RequestBodyType(typeof(NebulizerInput), "Nebulizer")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Nebulizer, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de Nebulizador
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("nebulizers_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(NebulizerInput))]
        public static async Task<IActionResult> NebulizerPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "nebulizers/{id}")]
            [RequestBodyType(typeof(NebulizerInput), "Nebulizer")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Nebulizer, id);
            return result.JsonResult;
        }

        /// <summary>
        /// Añadir Tractor
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("tractors_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TractorInput))]
        public static async Task<IActionResult> TractorPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tractors")]
            [RequestBodyType(typeof(TractorInput), "Tractor")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Tractor, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de Trabajo
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("tractors_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TractorInput))]
        public static async Task<IActionResult> TractorPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "tractors/{id}")]
            [RequestBodyType(typeof(TractorInput), "Tractor")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Tractor, id);
            return result.JsonResult;
        }



        /// <summary>
        /// Añadir Razón social
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("business_names_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BusinessNameInput))]
        public static async Task<IActionResult> BusinessNamePost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "business_name")]
            [RequestBodyType(typeof(BusinessNameInput), "BusinessName")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.BusinessName, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de Razón Social
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("business_names_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BusinessNameInput))]
        public static async Task<IActionResult> BusinessNamePut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "business_names/{id}")]
            [RequestBodyType(typeof(BusinessNameInput), "BusinessName")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.BusinessName, id);
            return result.JsonResult;

        }


        /// <summary>
        /// Añadir centro de costos
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("cost_centers_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CostCenterInput))]
        public static async Task<IActionResult> CostCenterPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "cost_centers")]
            [RequestBodyType(typeof(CostCenterInput), "CostCenter")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.CostCenter, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de centro de costos
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("cost_centers_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CostCenterInput))]
        public static async Task<IActionResult> CostCenterPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "cost_centers/{id}")]
            [RequestBodyType(typeof(CostCenterInput), "CostCenter")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.CostCenter, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Temporada
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("seasons_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SeasonInput))]
        public static async Task<IActionResult> SeasonPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "seasons")]
            [RequestBodyType(typeof(SeasonInput), "Season")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Season, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de Temporada
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("seasons_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SeasonInput))]
        public static async Task<IActionResult> SeasonPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "seasons/{id}")]
            [RequestBodyType(typeof(SeasonInput), "Season")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Season, id);
            return result.JsonResult;
        }

        /// <summary>
        /// Añadir Raíz
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("rootstock_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RootstockInput))]
        public static async Task<IActionResult> RootStockPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "rootstock")]
            [RequestBodyType(typeof(RootstockInput), "Rootstock")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Rootstock, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de Raíz
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("rootstock_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RootstockInput))]
        public static async Task<IActionResult> RootStockPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "rootstock/{id}")]
            [RequestBodyType(typeof(RootstockInput), "Rootstock")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Rootstock, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Carpeta de Órdenes
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("order_folders_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OrderFolderInput))]
        public static async Task<IActionResult> OrderFolderPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "order_folders")]
            [RequestBodyType(typeof(OrderFolderInput), "OrderFolder")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.OrderFolder, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de Carpeta de órdenes
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("order_folders_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OrderFolderInput))]
        public static async Task<IActionResult> OrderFolderPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "order_folders/{id}")]
            [RequestBodyType(typeof(OrderFolderInput), "OrderFolder")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.OrderFolder, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir cuartel
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("barracks_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BarrackInput))]
        public static async Task<IActionResult> BarracksPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "barracks")]
            [RequestBodyType(typeof(BarrackInput), "Barrack")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Barrack, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de Cuartel
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("barracks_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BarrackInput))]
        public static async Task<IActionResult> BarrackPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "barracks/{id}")]
            [RequestBodyType(typeof(BarrackInput), "Barrack")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Barrack, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Orden de aplicación
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("orders_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApplicationOrderInput))]
        public static async Task<IActionResult> OrderPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders")]
            [RequestBodyType(typeof(ApplicationOrderInput), "ApplicationOrder")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.ApplicationOrder, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de Orden de aplicación
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("orders_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApplicationOrderInput))]
        public static async Task<IActionResult> OrderPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "orders/{id}")]
            [RequestBodyType(typeof(ApplicationOrderInput), "ApplicationOrder")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.ApplicationOrder, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Pre Orden de aplicación
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("pre_orders_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PreOrderInput))]
        public static async Task<IActionResult> PreOrderPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "pre_orders")]
            [RequestBodyType(typeof(PreOrderInput), "PreOrder")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.PreOrder, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de Pre Orden de aplicación
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("pre_orders_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PreOrderInput))]
        public static async Task<IActionResult> PreOrderPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "pre_orders/{id}")]
            [RequestBodyType(typeof(PreOrderInput), "PreOrder")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.PreOrder, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Ejecución
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("executions_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExecutionOrderInput))]
        public static async Task<IActionResult> ExecutionsPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "executions")]
            [RequestBodyType(typeof(ExecutionOrderInput), "ExecutionOrder")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.ExecutionOrder, string.Empty);
            return result.JsonResult;
        }


        /// <summary>
        /// Modificación de Ejecución
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("executions_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExecutionOrderInput))]
        public static async Task<IActionResult> ExecutionsPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "executions/{id}")]
            [RequestBodyType(typeof(ExecutionOrderInput), "ExecutionOrder")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.ExecutionOrder, id);
            return result.JsonResult;
        }


        /// <summary>
        /// Añadir Ejecución
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("executions_status_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExecutionOrderStatusInput))]
        public static async Task<IActionResult> ExecutionsStatusPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "executions_status")]
            [RequestBodyType(typeof(ExecutionOrderStatusInput), "ExecutionOrderStatus")]
            HttpRequest req, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.ExecutionOrderStatus, string.Empty);
            return result.JsonResult;
        }




        /// <summary>
        /// Modificación de Estatus Ejecución
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("executions_status_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExecutionOrderStatusInput))]
        public static async Task<IActionResult> ExecutionsStatusPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "executions_status/{id}")]
            [RequestBodyType(typeof(ExecutionOrderStatusInput), "ExecutionOrderStatus")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.ExecutionOrderStatus, id);
            return result.JsonResult;
        }

      


    }

}