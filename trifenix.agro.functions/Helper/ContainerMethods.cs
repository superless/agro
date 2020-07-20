using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Spatial;
using System;
using System.Threading.Tasks;
using trifenix.agro.email.operations;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.operations;
using trifenix.agro.functions.settings;
using trifenix.agro.search.operations;
using trifenix.agro.storage.operations;
using trifenix.agro.weather.operations;
using trifenix.connect.mdm.containers;

namespace trifenix.agro.functions.Helper
{
    public static class ContainerMethods {
        public static async Task<IAgroManager<GeographyPoint>> AgroManager(string ObjectIdAAD, bool isBatch){
            var email = new Email("aresa.notificaciones@gmail.com", "Aresa2019");
            var uploadImage = new UploadImage(Environment.GetEnvironmentVariable("StorageConnectionStrings", EnvironmentVariableTarget.Process));
            var weatherApi = new WeatherApi(Environment.GetEnvironmentVariable("KeyWeatherApi", EnvironmentVariableTarget.Process));
            var searchServiceInstance = new AgroSearch<GeographyPoint>(Environment.GetEnvironmentVariable("SearchServiceName", EnvironmentVariableTarget.Process), Environment.GetEnvironmentVariable("SearchServiceKey", EnvironmentVariableTarget.Process), new CorsOptions(Environment.GetEnvironmentVariable("Search_allowedOrigins", EnvironmentVariableTarget.Process).Split(";")));
            return new AgroManager(ConfigManager.GetDbArguments, email, uploadImage, weatherApi, searchServiceInstance, ObjectIdAAD, isBatch);
        }

        public static JsonResult GetJsonPostContainer<T>(ExtPostContainer<T> containerResponse, ILogger log){
            if (containerResponse.GetType() == typeof(ExtPostErrorContainer<T>)) {
                var resultError = (ExtPostErrorContainer<T>)containerResponse;
                log.LogError(resultError.InternalException, string.Join(Environment.NewLine, resultError.ValidationMessages));
                                        //TODO: Revisar
                return new JsonResult(resultError.ValidationMessages.Count > 0? (object)resultError.ValidationMessages : resultError.GetBase);
            }
            return new JsonResult(containerResponse);
        }

        public static JsonResult GetJsonGetContainer<T>(ExtGetContainer<T> containerResponse, ILogger log)  {
            if (containerResponse.GetType() == typeof(ExtGetErrorContainer<T>)) {
                var resultError = (ExtGetErrorContainer<T>)containerResponse;
                log.LogError(resultError.InternalException, resultError.ErrorMessage);
                return new JsonResult(resultError.GetBase);
            }
            return new JsonResult(containerResponse);
        }

    }

}