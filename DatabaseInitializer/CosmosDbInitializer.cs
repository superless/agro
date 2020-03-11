using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using trifenix.agro.cosmosdbinitializer.interfaces;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.authentication.operations
{
    public class CosmosDbInitializer : ICosmosDbInitializer {

        //private static CosmosStoreSettings StoreSettings;
        private readonly Assembly Assembly;
        private readonly IAgroManager Manager;

        public CosmosDbInitializer(IAgroManager manager, string AssemblyName) {
            Manager = manager;
            //StoreSettings = new CosmosStoreSettings(CosmosDbName ?? "agrodb", CosmosDbUri ?? "https://agricola-jhm.documents.azure.com:443", CosmosDbPrimaryKey ?? "yG6EIAT1dKSBaS7oSZizTrWQGGfwSb2ot2prYJwQOLHYk3cGmzvvhGohSzFZYHueSFDiptUAqCQYYSeSetTiKw==");
            Assembly = !string.IsNullOrWhiteSpace(AssemblyName) ? Assembly.Load(AssemblyName) : typeof(BusinessNameInput).Assembly;
        }

        //public static CosmosStore<T> CreateStoreInstance<T>() where T : class => new CosmosStore<T>(StoreSettings);

        public static T CreateEntityInputInstance<T>() => (T)Activator.CreateInstance(typeof(T));

        public static T GetValue<T>(JValue value) => typeof(T).Equals(typeof(DateTime)) ? (T)(object)DateTime.Parse(value.Value<string>()) : value.Value<T>();

        public async void MapJsonToDB(dynamic json) {
            if (!(json is JObject))
                throw new Exception("\nError! Se espera un Json.");
            Type entityInputType;
            PropertyInfo prop;
            //dynamic Store;
            dynamic dbInputInstance, EntityOperations;
            List<string> Guids = new List<string>();
            string idRelated;
            foreach (var entity in json) {
                entityInputType = Assembly.GetTypes().SingleOrDefault(type => type.Name.Equals($"{entity.Name}Input"));
                if (entityInputType == null)
                    continue;
                EntityOperations = Manager.GetType().GetProperty(entity.Name).GetValue(Manager);
                //Store = typeof(CosmosDbInitializer).GetMethod("CreateStoreInstance").MakeGenericMethod(entityType).Invoke(null, null);
                dbInputInstance = typeof(CosmosDbInitializer).GetMethod("CreateEntityInputInstance").MakeGenericMethod(entityInputType).Invoke(null, null);
                foreach (var jsonInstance in entity.Value) {
                    var properties = jsonInstance.Properties();
                    foreach (var p in properties) {
                        prop = entityInputType.GetProperty(p.Name);
                        var value = typeof(CosmosDbInitializer).GetMethod("GetValue").MakeGenericMethod(prop.PropertyType).Invoke(null, new object[] { p.Value });
                        int index = GetNumberOfGuid(value.ToString());
                        if(index != -1 && index < Guids.Count)
                            value = Guids.ElementAt(index);
                        prop?.SetValue(dbInputInstance, value);
                    }
                    try {
                        idRelated = (await EntityOperations.Save(dbInputInstance)).IdRelated;
                        Guids.Add(idRelated);
                    } catch(Exception e) {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        private int GetNumberOfGuid(string InputGuid) {
            string[] input = InputGuid.Split('(', ')');
            return input.Length == 3 && input[0].Equals("Guid") && int.TryParse(input[1], out int result) && result >=0 && input[2].Equals("")? result : -1;
        }

    }

}