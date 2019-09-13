using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using trifenix.agro.applicationsReference;
using trifenix.agro.db.applicationsReference;
using trifenix.agro.functions.settings;
using trifenix.agro.model.external;
using trifenix.agro.model.external.interfaces;

namespace trifenix.agro.functions.Helper
{
    public static class ContainerMethods
    {
        public static IReferenceApplications ContainerDbApplication => new ReferenceApplications(new BaseContainers(ConfigManager.GetDbArguments));

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
