using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.functions.Helper;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.functions.mantainers
{
    public class ActionResultWithId
    {
        public string Id { get; set; }
        public ActionResult JsonResult { get; set; }

    }


    public static class GenericMantainer<InputElement,DbElement> where InputElement : InputBase where DbElement : DocumentBase
    {

        public static async Task<ActionResultWithId> HttpProcessing(HttpRequest req, ILogger log, IExistElement existsElement, IGenericOperation<DbElement, InputElement> repo, IGenericOperation<UserActivity, UserActivityInput> recordAcitvity, InputElement element) {
            var method = req.Method.ToLower();

            switch (method)
            {
                case "get":
                    if (!string.IsNullOrWhiteSpace(element.Id))
                    {
                        var elementDb = await repo.Get(element.Id);
                        return new ActionResultWithId { 
                            Id = element.Id,
                            JsonResult = ContainerMethods.GetJsonGetContainer(elementDb, log)
                        };
                    }

                    return new ActionResultWithId { 
                        Id = null,
                        JsonResult = ContainerMethods.GetJsonGetContainer(new ExtGetContainer<string> { ErrorMessage = "id obligatorio", StatusResult = ExtGetDataResult.Error }, log)
                    };



                default:
                    ExtPostContainer<string> saveReturn;
                    try
                    {
                        saveReturn = await repo.Save(element);

                    }
                    catch (Exception ex)
                    {
                        return new ActionResultWithId { 
                            Id = null,
                            JsonResult = ContainerMethods.GetJsonPostContainer(new ExtPostErrorContainer<string>
                            {
                                InternalException = ex,
                                Message = ex.Message,
                                MessageResult = ExtMessageResult.Error
                            }, log)
                        };
                    }

                    if (!string.IsNullOrWhiteSpace(saveReturn.IdRelated))
                    {
                        await recordAcitvity.Save(new UserActivityInput
                        {
                            Action = method.Equals("post") ? UserActivityAction.CREATE : UserActivityAction.MODIFY,
                            Date = DateTime.Now,
                            EntityName = ((DbElement)Activator.CreateInstance(typeof(DbElement))).CosmosEntityName,
                            Id = saveReturn.IdRelated
                        });
                    }
                    return new ActionResultWithId { 
                        Id= saveReturn.IdRelated,
                        JsonResult = ContainerMethods.GetJsonPostContainer(saveReturn, log)
                    };
            }

        }

        public static async Task<ActionResultWithId> SendInternalHttp(HttpRequest req, ILogger log, Func<IAgroManager, IGenericOperation<DbElement, InputElement>> repo, string id = null)
        {

            ClaimsPrincipal claims = await Auth.Validate(req);

            if (claims == null)
                return new ActionResultWithId
                {
                    JsonResult = new UnauthorizedResult()
                };


            var manager = await ContainerMethods.AgroManager(claims);

            return await HttpProcessing(req, log, manager.ExistsElements, repo(manager), manager.UserActivity, id);



        }

        public static async Task<ActionResultWithId> HttpProcessing(HttpRequest req, ILogger log, IExistElement existsElement, IGenericOperation<DbElement, InputElement> repo, IGenericFullReadOperation<UserActivity, UserActivityInput> recordAcitvity,  string id = null ) {


            var jsonElement = await ConvertToElement(req, id);
            
            return await HttpProcessing(req, log, existsElement, repo, recordAcitvity, jsonElement);

        }

        public static async Task<InputElement> ConvertToElement(HttpRequest req, string id) 
        {

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            InputElement element;
            if (req.Method.ToLower().Equals("get"))
            {
                element = (InputElement)Activator.CreateInstance(typeof(InputElement));

            }
            else
            {
                element = JsonConvert.DeserializeObject<InputElement>(requestBody);
            }

            element.Id = id;
            return element;
        }

        
    }
}
