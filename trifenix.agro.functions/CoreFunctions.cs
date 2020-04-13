using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using trifenix.agro.authentication.operations;
using trifenix.agro.db.exceptions;
using trifenix.agro.enums.input;
using trifenix.agro.enums.model;
using trifenix.agro.external.operations.helper;
using trifenix.agro.functions.Helper;
using trifenix.agro.functions.mantainers;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.servicebus.operations;
using trifenix.agro.swagger.model.input;

namespace trifenix.agro.functions {

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
        public static async Task<IActionResult> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] [RequestBodyType(typeof(LoginInput), "Nombre de usuario y contraseña")] HttpRequest req, ILogger log) {
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

        [FunctionName("ServiceBus")]
        public static async Task Handler([ServiceBusTrigger("agroqueue", Connection = "ServiceBusConnectionString", IsSessionsEnabled = true)]Message message, ILogger log) {
            var opInstance = ServiceBus.Deserialize(message.Body);
            var agro = await ContainerMethods.AgroManager(opInstance.Value<string>("ObjectIdAAD"), false);
            var entityType = opInstance["EntityType"].ToObject<Type>();
            var repo = agro.GetOperationByInputType(entityType);
            dynamic element = opInstance["Element"].ToObject(entityType);
            try {
                var saveReturn = await repo.SaveInput(element, false);
                var recordActivity = agro.GetOperationByInputType(typeof(UserActivityInput));
                await recordActivity.SaveInput(new UserActivityInput {
                    Action = opInstance.Value<string>("HttpMethod").Equals("post") ? UserActivityAction.CREATE : UserActivityAction.MODIFY,
                    Date = DateTime.Now,
                    EntityName = opInstance.Value<string>("EntityName"),
                    EntityId = saveReturn.IdRelated
                }, false);
            }
            catch (Exception ex) {
                var extPostError = new ExtPostErrorContainer<string> {
                    InternalException = ex,
                    Message = ex.Message,
                    MessageResult = ExtMessageResult.Error
                };
                if (ex is Validation_Exception)
                    extPostError.ValidationMessages = ((Validation_Exception)ex).ErrorMessages;
                log.LogError(extPostError.InternalException, extPostError.Message);
                //return new ActionResultWithId {
                //    Id = null,
                //    JsonResult = ContainerMethods.GetJsonPostContainer(extPostError, log)
                //};
            }
        }

        /// <summary>
        /// Creación de Sector
        /// </summary>
        /// <return>
        /// Retorna un contenedor con el id
        /// </return>
        [FunctionName("sector_post")]
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> PlotLandsPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "plotlands")]
            [RequestBodyType(typeof(PlotLandInput), "Parcela")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> PlotLandsPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "plotlands/{id}")]
            [RequestBodyType(typeof(PlotLandInput), "Parcela")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> SpeciesPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "species")]
            [RequestBodyType(typeof(SpecieInput), "Especie")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> SpeciesPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "species/{id}")]
            [RequestBodyType(typeof(SpecieInput), "Especie")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> NotificationsPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "notification")]
            [RequestBodyType(typeof(NotificationEventInput), "notification")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> NotificationPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "notification/{id}")]
            [RequestBodyType(typeof(NotificationEventInput), "Notificación")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> VarietyPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "varieties")]
            [RequestBodyType(typeof(VarietyInput), "Variedad")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> VarietyPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "varieties/{id}")]
            [RequestBodyType(typeof(VarietyInput), "Variedad")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> TargetPost(

            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "targets")]
            [RequestBodyType(typeof(ApplicationTargetInput), "Objetivo de aplicación")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> TargetPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "targets/{id}")]
            [RequestBodyType(typeof(ApplicationTargetInput), "Objetivo de aplicación")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> PhenologicalEventPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "phenological_events")]
            [RequestBodyType(typeof(PhenologicalEventInput), "Evento Fenológico")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> PhenologicalEventPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "phenological_events/{id}")]
            [RequestBodyType(typeof(PhenologicalEventInput), "Evento Fenológico")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> CertifiedEntityPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "certified_entities")]
            [RequestBodyType(typeof(CertifiedEntityInput), "Entidad Certificadora")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> CertifiedEntityPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "certified_entities/{id}")]
            [RequestBodyType(typeof(CertifiedEntityInput), "Entidad Certificadora")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> CategoryIngredientPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ingredient_categories")]
            [RequestBodyType(typeof(IngredientCategoryInput), "Categoría de Ingrediente")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> CategoryIngredientPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ingredient_categories/{id}")]
            [RequestBodyType(typeof(IngredientCategoryInput), "Categoría de Ingrediente")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> IngredientsPost(

            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ingredients")]
            [RequestBodyType(typeof(IngredientInput), "Ingrediente")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> IngredientPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ingredients/{id}")]
            [RequestBodyType(typeof(IngredientInput), "Ingrediente")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public async static Task<IActionResult> ProductsPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")]
            [RequestBodyType(typeof(ProductInput), "Producto")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> ProductPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "products/{id}")]
            [RequestBodyType(typeof(ProductInput), "Producto")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> RolePost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "roles")]
            [RequestBodyType(typeof(RoleInput), "Rol")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> RolePut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "roles/{id}")]
            [RequestBodyType(typeof(RoleInput), "Rol")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> JobPost(

            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "jobs")]
            [RequestBodyType(typeof(JobInput), "Trabajo")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> JobPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "jobs/{id}")]
            [RequestBodyType(typeof(JobInput), "Trabajo")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> UserPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")]
            [RequestBodyType(typeof(UserApplicatorInput), "Usuario aplicador")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> UsersPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "users/{id}")]
            [RequestBodyType(typeof(UserApplicatorInput), "Usuario aplicador")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> NebulizersPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "nebulizers")]
            [RequestBodyType(typeof(NebulizerInput), "Nebulizador")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> NebulizerPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "nebulizers/{id}")]
            [RequestBodyType(typeof(NebulizerInput), "Nebulizador")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        public static async Task<IActionResult> BusinessNamePost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "business_names")]
            [RequestBodyType(typeof(BusinessNameInput), "Razón social")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> BusinessNamePut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "business_names/{id}")]
            [RequestBodyType(typeof(BusinessNameInput), "Razón Social")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> CostCenterPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "cost_centers")]
            [RequestBodyType(typeof(CostCenterInput), "Centro de Costos")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> CostCenterPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "cost_centers/{id}")]
            [RequestBodyType(typeof(CostCenterInput), "Centro de Costos")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> SeasonPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "seasons")]
            [RequestBodyType(typeof(SeasonInput), "Temporada")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> SeasonPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "seasons/{id}")]
            [RequestBodyType(typeof(SeasonInput), "Temporada")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> RootStockPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "rootstock")]
            [RequestBodyType(typeof(RootstockInput), "Raíz")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> RootStockPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "rootstock/{id}")]
            [RequestBodyType(typeof(RootstockInput), "Raíz")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> OrderFolderPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "order_folders")]
            [RequestBodyType(typeof(OrderFolderInput), "Carpeta de órdenes")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> OrderFolderPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "order_folders/{id}")]
            [RequestBodyType(typeof(OrderFolderInput), "Carpeta de órdenes")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> BarracksPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "barracks")]
            [RequestBodyType(typeof(BarrackInput), "Cuartel")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> BarrackPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "barracks/{id}")]
            [RequestBodyType(typeof(BarrackInput), "Cuartel")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> OrderPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders")]
            [RequestBodyType(typeof(ApplicationOrderInput), "Orden de aplicación")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> OrderPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "orders/{id}")]
            [RequestBodyType(typeof(ApplicationOrderInput), "Orden de aplicación")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> PreOrderPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "pre_orders")]
            [RequestBodyType(typeof(PreOrderInput), "Pre Orden de aplicación")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> PreOrderPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "pre_orders/{id}")]
            [RequestBodyType(typeof(PreOrderInput), "Pre Orden de aplicación")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> ExecutionsPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "executions")]
            [RequestBodyType(typeof(ExecutionOrderInput), "Ejecución aplicación")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> ExecutionsPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "executions/{id}")]
            [RequestBodyType(typeof(ExecutionOrderInput), "Ejecución aplicación")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> ExecutionsStatusPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "executions_status")]
            [RequestBodyType(typeof(ExecutionOrderStatusInput), "Estatus Ejecución aplicación")]
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
        [RequestHttpHeader("Authorization", isRequired: true)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExtGetContainer<string>))]
        public static async Task<IActionResult> ExecutionsStatusPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "executions_status/{id}")]
            [RequestBodyType(typeof(ExecutionOrderStatusInput), "Ejecución aplicación")]
            HttpRequest req, string id, ILogger log)
        {
            var result = await GenericMantainer.SendInternalHttp(req, log, s => s.ExecutionOrderStatus, id);
            return result.JsonResult;
        }

        [FunctionName("Initialize")]
        public static async Task<IActionResult> Initialize([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req, ILogger log) {
            var claims = await Auth.Validate(req);
            if (claims == null)
                return new UnauthorizedResult();
            string ObjectIdAAD = claims.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var manager = await ContainerMethods.AgroManager(ObjectIdAAD, true);
            string json = await req.ReadAsStringAsync();
            JObject jsonObject = JObject.Parse(json);
            var dbInitializer = new CosmosDbInitializer(manager, jsonObject.Value<string>("Assembly_Inputs"), jsonObject.Value<string>("Assembly_Entities"));
            var result = await dbInitializer.MapJsonToDB(jsonObject.Value<JObject>("Entities"));
            return result;
        }

    }

}