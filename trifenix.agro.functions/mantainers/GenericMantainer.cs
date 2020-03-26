using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.enums.input;
using trifenix.agro.enums.model;
using trifenix.agro.external.interfaces;
using trifenix.agro.functions.Helper;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.functions.mantainers {

    public static class GenericMantainer {

        public static async Task<ActionResultWithId> SendInternalHttp<DbElement, InputElement>(HttpRequest req, ILogger log, Func<IAgroManager, IGenericOperation<DbElement, InputElement>> repo, string id = null) where DbElement : DocumentBase where InputElement : InputBase {
            var claims = await Auth.Validate(req);
            if (claims == null)
                return new ActionResultWithId {
                    Id = null,
                    JsonResult = new UnauthorizedResult()
                };
            string ObjectIdAAD = claims.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var manager = await ContainerMethods.AgroManager(ObjectIdAAD);
            return await HttpProcessing(req, log, repo(manager), manager.UserActivity, id);
        }

        public static async Task<ActionResultWithId> HttpProcessing<DbElement, InputElement>(HttpRequest req, ILogger log, IGenericOperation<DbElement, InputElement> repo, IGenericOperation<UserActivity, UserActivityInput> recordAcitvity, string id = null) where DbElement : DocumentBase where InputElement : InputBase {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var method = req.Method.ToLower();
            var jsonElement = ConvertToElement<InputElement>(body, id, method);
            return await HttpProcessing(req, log, repo, recordAcitvity, jsonElement);
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
                catch (Exception) {
                    throw;
                }
            element.Id = id;
            return element;
        }

        public static async Task<ActionResultWithId> HttpProcessing<DbElement, InputElement>(HttpRequest req, ILogger log, IGenericOperation<DbElement, InputElement> repo, IGenericOperation<UserActivity, UserActivityInput> recordAcitvity, InputElement element) where DbElement : DocumentBase where InputElement : InputBase {
            var method = req.Method.ToLower();
            switch (method) {
                case "get":
                    if (!string.IsNullOrWhiteSpace(element.Id)) {
                        var elementDb = await repo.Get(element.Id);
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
                    ExtPostContainer<string> saveReturn;
                    try {
                        saveReturn = await repo.Save(element);
                        await recordAcitvity.Save(new UserActivityInput
                        {
                            Action = method.Equals("post") ? UserActivityAction.CREATE : UserActivityAction.MODIFY,
                            Date = DateTime.Now,
                            EntityName = ((DbElement)Activator.CreateInstance(typeof(DbElement))).CosmosEntityName,
                            EntityId = saveReturn.IdRelated
                        });
                    }
                    catch (Exception ex) {
                        return new ActionResultWithId {
                            Id = null,
                            JsonResult = ContainerMethods.GetJsonPostContainer(new ExtPostErrorContainer<string> {
                                InternalException = ex,
                                Message = ex.Message,
                                MessageResult = ExtMessageResult.Error
                            }, log)
                        };
                    }
                    return new ActionResultWithId {
                        Id = saveReturn.IdRelated,
                        JsonResult = ContainerMethods.GetJsonPostContainer(saveReturn, log)
                    };
            }
        }

    }

    public class ActionResultWithId {

        public string Id { get; set; }
        public ActionResult JsonResult { get; set; }

    }

}