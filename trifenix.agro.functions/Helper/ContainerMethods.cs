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
using trifenix.agro.storage.operations;

namespace trifenix.agro.functions.Helper
{
    public static class ContainerMethods
    {
        

        public static async Task<IAgroManager> AgroManager(){
           
            var agroDb = new AgroRepository(ConfigManager.GetDbArguments);
            var season = await agroDb.Seasons.GetCurrentSeason();
            var uploadImage = new UploadImage(Environment.GetEnvironmentVariable("StorageConnectionStrings", EnvironmentVariableTarget.Process));
            return new AgroManager(agroDb, season?.Id,uploadImage); 
        
        }


        public static async Task<JsonResult> ApiPostOperations<T>(Stream body, ILogger log, Func<IAgroManager, dynamic, Task<ExtPostContainer<T>>> create) 
        {
            try
            {
                var requestBody = await new StreamReader(body).ReadToEndAsync();
                dynamic result = JsonConvert.DeserializeObject(requestBody);

                var manager = await AgroManager();
                var resultDb = await create(manager, result);
                return ContainerMethods.GetJsonPostContainer(resultDb, log);
            }
            catch (Exception ex)
            {
                return GetJsonPostContainer(new ExtPostErrorContainer<string>
                {
                    InternalException = ex,
                    Message = ex.Message,
                    MessageResult = ExtMessageResult.Error
                }, log);
            }
           
        }


        public static JsonResult GetJsonPostContainer<T>(ExtPostContainer<T> containerResponse, ILogger log){
            if (containerResponse.GetType() == typeof(ExtPostErrorContainer<T>))
            {
                var resultError = (ExtPostErrorContainer<T>)containerResponse;
                log.LogError(resultError.InternalException, resultError.Message);
                return new JsonResult(resultError.GetBase);

            }

            return new JsonResult(containerResponse);
        }

        public static JsonResult GetJsonGetContainer<T>(ExtGetContainer<T> containerResponse, ILogger log)
        {
            if (containerResponse.GetType() == typeof(ExtGetErrorContainer<T>))
            {
                var resultError = (ExtGetErrorContainer<T>)containerResponse;
                log.LogError(resultError.InternalException, resultError.ErrorMessage);
                return new JsonResult(resultError.GetBase);

            }

            return new JsonResult(containerResponse);
        }
    }
}
