using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.applicationsReference;
using trifenix.agro.db;
using trifenix.agro.db.applicationsReference;
using trifenix.agro.db.applicationsReference.agro;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations;
using trifenix.agro.functions.settings;
using trifenix.agro.model.external;

namespace trifenix.agro.functions.Helper
{
    public static class ContainerMethods
    {
        public static IReferenceApplications ContainerDbApplication => new ReferenceApplications(new BaseContainers(ConfigManager.GetDbArguments));

        public static IAgroManager AgroManager => new AgroManager(new AgroRepository(ConfigManager.GetDbArguments));


        public static async Task<JsonResult> ApiPostOperations<T>(Stream body, ILogger log, Func<IAgroManager, dynamic, Task<ExtPostContainer<T>>> create) 
        {
            var requestBody = await new StreamReader(body).ReadToEndAsync();
            dynamic result = JsonConvert.DeserializeObject(requestBody);
            var resultDb = await create(AgroManager, result);
            return ContainerMethods.GetJsonPostContainer(resultDb, log);
           
        }


        public static JsonResult GetJsonPostContainer<T>(ExtPostContainer<T> containerResponse, ILogger log){
            if (containerResponse.GetType() == typeof(ExtPostErrorContainer<T>))
            {
                var resultError = (ExtPostErrorContainer<T>)containerResponse;
                log.LogError(resultError.InternalException, resultError.Message, JsonConvert.SerializeObject(resultError));
                return new JsonResult(resultError.GetBase);

            }

            return new JsonResult(containerResponse);
        }

        public static JsonResult GetJsonGetContainer<T>(ExtGetContainer<T> containerResponse, ILogger log)
        {
            if (containerResponse.GetType() == typeof(ExtGetErrorContainer<T>))
            {
                var resultError = (ExtGetErrorContainer<T>)containerResponse;
                log.LogError(resultError.InternalException, resultError.ErrorMessage, JsonConvert.SerializeObject(resultError));
                return new JsonResult(resultError.GetBase);

            }

            return new JsonResult(containerResponse);
        }
    }
}
