using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

using trifenix.connect.agro.external.helper;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.queries;
using trifenix.connect.agro_model_input;
using trifenix.connect.aad.auth;
using trifenix.connect.bus;
using trifenix.connect.db.cosmos.exceptions;
using trifenix.connect.interfaces.auth;
using trifenix.connect.mdm.containers;

namespace trifenix.agro.functions
{

    /// <summary>
    /// Funciones 
    /// </summary>
    public static class CoreFunctions {

        /// <summary>
        /// Login, donde usa usuario y contraseña para obtener el token.
        /// </summary>
        /// <param name="req">cabecera que debe incluir el modelo de entrada </param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("Login")]        
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log) {
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

        [FunctionName("MessagesNegotiateBinding")]
        public static async Task<IActionResult> NegotiatBindingAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "negotiate")] HttpRequest req,
        IBinder binder,
        ILogger log) {

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
            else {
                // temporal, para conectar winform sin autenticación
                return new UnauthorizedResult();

            }

        }

        

        [FunctionName("ServiceBus")]
        public static async Task Handler(
        [ServiceBusTrigger("trifenix-agrobus", Connection = "ServiceBusConnectionString", IsSessionsEnabled = true)]Message message,
        [SignalR(HubName = "agro")]IAsyncCollector<SignalRMessage> signalRMessages,
        ILogger log) {
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

            try {
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
                
                await signalRMessages.AddAsync(new SignalRMessage { Target = "Success", UserId = userId??"cloud-app", Arguments = new object[] { EntityName } });
            }
            catch (Exception ex) {
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> SectorPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "sectors")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> PlotLandsPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "plotlands")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> PlotLandsPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "plotlands/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> SpeciesPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "species")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> SpeciesPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "species/{id}")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.Specie, id);
            return result.JsonResult;
        }

        /// <summary>
        /// Añadir Notificación
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("notification_event_post")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> NotificationsPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "notification")]
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
        [FunctionName("notification_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> NotificationPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "notification/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> VarietyPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "varieties")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> VarietyPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "varieties/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> TargetPost(

            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "targets")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> TargetPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "targets/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> PhenologicalEventPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "phenological_events")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> PhenologicalEventPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "phenological_events/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> CertifiedEntityPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "certified_entities")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> CertifiedEntityPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "certified_entities/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> CategoryIngredientPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ingredient_categories")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> CategoryIngredientPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ingredient_categories/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> IngredientsPost(

            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ingredients")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> IngredientPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ingredients/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public async static Task<IActionResult> ProductsPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> ProductPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "products/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> RolePost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "roles")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> RolePut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "roles/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> JobPost(

            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "jobs")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> JobPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "jobs/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> NebulizersPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "nebulizers")]
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
        [FunctionName("nebulizer_put")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> NebulizerPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "nebulizers/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> TractorPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tractors")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> TractorPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "tractors/{id}")]
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
        public static async Task<IActionResult> BusinessNamePost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "business_names")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> BusinessNamePut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "business_names/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> CostCenterPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "cost_centers")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> CostCenterPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "cost_centers/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> SeasonPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "seasons")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> SeasonPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "seasons/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> RootStockPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "rootstock")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> RootStockPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "rootstock/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> OrderFolderPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "order_folders")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> OrderFolderPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "order_folders/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> BarracksPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "barracks")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> BarrackPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "barracks/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> OrderPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> OrderPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "orders/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> PreOrderPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "pre_orders")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> PreOrderPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "pre_orders/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> ExecutionsPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "executions")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> ExecutionsPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "executions/{id}")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> ExecutionsStatusPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "executions_status")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> ExecutionsStatusPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "executions_status/{id}")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.ExecutionOrderStatus, id);
            return result.JsonResult;
        }

        

    }

}