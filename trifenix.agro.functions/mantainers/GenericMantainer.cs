using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Spatial;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using trifenix.agro.functions.Helper;

using trifenix.connect.agro.interfaces.external;
using trifenix.connect.bus;
using trifenix.connect.entities.cosmos;
using trifenix.connect.input;
using trifenix.connect.interfaces.external;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.enums;

namespace trifenix.agro.functions.mantainers
{

    public static class GenericMantainer {

        private static readonly ServiceBus ServiceBus = new ServiceBus(Environment.GetEnvironmentVariable("ServiceBusConnectionString", EnvironmentVariableTarget.Process), Environment.GetEnvironmentVariable("QueueName", EnvironmentVariableTarget.Process));

        private static void ValidaEnvironmentVariables() {
            var serviceBus = Environment.GetEnvironmentVariable("ServiceBusConnectionString", EnvironmentVariableTarget.Process);
            var queuName = Environment.GetEnvironmentVariable("QueueName", EnvironmentVariableTarget.Process);
            var storage = Environment.GetEnvironmentVariable("StorageConnectionStrings", EnvironmentVariableTarget.Process);
            var weatherApi = Environment.GetEnvironmentVariable("KeyWeatherApi", EnvironmentVariableTarget.Process);
            var searchServiceUrl = Environment.GetEnvironmentVariable("SearchServiceName", EnvironmentVariableTarget.Process);
            var serviceKey = Environment.GetEnvironmentVariable("SearchServiceKey", EnvironmentVariableTarget.Process);
            var cosmosDbName = Environment.GetEnvironmentVariable("CosmosDbName", EnvironmentVariableTarget.Process);
            var cosmosDbPrimaryKey = Environment.GetEnvironmentVariable("CosmosDbPrimaryKey", EnvironmentVariableTarget.Process);
            var cosmosDbUri = Environment.GetEnvironmentVariable("CosmosDbUri", EnvironmentVariableTarget.Process);
            var SearchServiceName = Environment.GetEnvironmentVariable("SearchServiceName", EnvironmentVariableTarget.Process);

            if (string.IsNullOrWhiteSpace(serviceBus))
            {
                throw new Exception("Falta el bus aweonao");
            }
            if (string.IsNullOrWhiteSpace(queuName))
            {
                throw new Exception("Falta el queuename aweonao");
            }
            if (string.IsNullOrWhiteSpace(storage))
            {
                throw new Exception("Falta el storage aweonao");
            }
            if (string.IsNullOrWhiteSpace(weatherApi))
            {
                throw new Exception("Falta la weatherApi aweonao");
            }
            if (string.IsNullOrWhiteSpace(searchServiceUrl))
            {
                throw new Exception("Falta el searchServiceUrl aweonao");
            }
            if (string.IsNullOrWhiteSpace(serviceKey))
            {
                throw new Exception("Falta el serviceKey aweonao");
            }
            if (string.IsNullOrWhiteSpace(cosmosDbName))
            {
                throw new Exception("Falta el cosmoDbName aweonao");
            }
            if (string.IsNullOrWhiteSpace(cosmosDbPrimaryKey))
            {
                throw new Exception("Falta el cosmoDbPrimaryKey aweonao");
            }
            if (string.IsNullOrWhiteSpace(cosmosDbUri))
            {
                throw new Exception("Falta el CosmoDbUri aweonao");
            }
            
        }

        public static async Task<ActionResultWithId> SendInternalHttp<DbElement, InputElement>(HttpRequest req, ILogger log, Func<IAgroManager<GeographyPoint>, IGenericOperation<DbElement, InputElement>> repo, string id = null) where DbElement : DocumentBase where InputElement : InputBase {
            ValidaEnvironmentVariables();
            //if (!ValidaEnvironmentVariables()) throw new Exception("existen variables de ambiente nulas, por favor revise las variables");

            //var claims = await Auth.Validate(req);
            //if (claims == null)
            //    return new ActionResultWithId {
            //        Id = null,
            //        JsonResult = new UnauthorizedResult()
            //    };
            //string ObjectIdAAD = claims.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            string ObjectIdAAD = string.Empty;
            return await HttpProcessing(req, log, ObjectIdAAD, repo, id);
        }

        public static async Task<ActionResultWithId> HttpProcessing<DbElement, InputElement>(HttpRequest req, ILogger log, string ObjectIdAAD, Func<IAgroManager<GeographyPoint>, IGenericOperation<DbElement, InputElement>> repo, string id = null) where DbElement : DocumentBase where InputElement : InputBase {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var method = req.Method.ToLower();
            

            var inputElement = ConvertToElement<InputElement>(body, id, method);
            
            return await HttpProcessing(req, log, ObjectIdAAD, repo, inputElement);
        }

        public static InputElement ConvertToElement<InputElement>(string body, string id, string method) where InputElement : InputBase {
            var requestBody = body;
            InputElement element;
            if (method.Equals("get"))
                element = (InputElement)Activator.CreateInstance(typeof(InputElement));
            else
                try {
                    element = JsonConvert.DeserializeObject<InputElement>(requestBody);
                }
                catch (Exception e) {
                    Console.WriteLine($"error en {e}");
                    throw;
                }
            element.Id = id;
            return element;
        }

        public static async Task<ActionResultWithId> HttpProcessing<DbElement, InputElement>(HttpRequest req, ILogger log, string ObjectIdAAD, Func<IAgroManager<GeographyPoint>, IGenericOperation<DbElement, InputElement>> repo, InputElement element) where DbElement : DocumentBase where InputElement : InputBase {
            var method = req.Method.ToLower();
            switch (method) {
                case "get":
                    if (!string.IsNullOrWhiteSpace(element.Id)) {
                        var manager = await ContainerMethods.AgroManager(ObjectIdAAD);
                        var elementDb = await repo(manager).Get(element.Id);
                        return new ActionResultWithId {
                            Id = element.Id,
                            JsonResult = ContainerMethods.GetJsonGetContainer(elementDb, log)
                        };
                    }
                    return new ActionResultWithId {
                        Id = null,
                        JsonResult = ContainerMethods.GetJsonGetContainer(new ExtGetContainer<string> { ErrorMessage = "Id obligatorio", StatusResult = ExtGetDataResult.Error }, log)
                    };
                default:
                    
                    string EntityName = ((DbElement)Activator.CreateInstance(typeof(DbElement))).CosmosEntityName;
                    
                    var opInstance = new OperationInstance<InputElement>(element, element.Id, EntityName, method, ObjectIdAAD);


                    try
                    {
                        await ServiceBus.PushElement(opInstance, EntityName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("error en el service bus");
                    }

                    return new ActionResultWithId {
                        Id = null,
                        JsonResult = new JsonResult("Operacion en cola.")
                    };
                    
            }
        }

    }

    public class ActionResultWithId {

        public string Id { get; set; }
        public ActionResult JsonResult { get; set; }

    }

}