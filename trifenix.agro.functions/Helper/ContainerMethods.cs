using Azure.Search.Documents.Indexes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Spatial;
using System;
using System.Threading.Tasks;
using trifenix.agro.external;
using trifenix.agro.functions.settings;
using trifenix.agro.storage.operations;
using trifenix.agro.weather.operations;
using trifenix.connect.agro.external;
using trifenix.connect.agro.external.hash;
using trifenix.connect.agro.external.helper;
using trifenix.connect.agro.interfaces.external;
using trifenix.connect.email;
using trifenix.connect.mdm.containers;
using trifenix.connect.mdm.search.model;

namespace trifenix.agro.functions.Helper
{
    public static class ContainerMethods {
        public static async Task<IAgroManager<GeographyPoint>> AgroManager(string ObjectIdAAD){
            var email = new Email("aresa.notificaciones@gmail.com", "Aresa2019");
            var uploadImage = new UploadImage(Environment.GetEnvironmentVariable("StorageConnectionStrings", EnvironmentVariableTarget.Process));
            var weatherApi = new WeatherApi(Environment.GetEnvironmentVariable("KeyWeatherApi", EnvironmentVariableTarget.Process));
            var searchServiceInstance = new AgroSearch<GeographyPoint>(Environment.GetEnvironmentVariable("SearchServiceName", EnvironmentVariableTarget.Process), Environment.GetEnvironmentVariable("SearchServiceKey", EnvironmentVariableTarget.Process), new ImplementsSearch(), new HashEntityAgroSearch());
            return new AgroManager<GeographyPoint>(new DbConnect(ConfigManager.GetDbArguments), email, uploadImage, weatherApi, searchServiceInstance, ObjectIdAAD);
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