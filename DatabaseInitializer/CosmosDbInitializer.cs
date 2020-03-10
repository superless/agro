using Cosmonaut;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using trifenix.agro.cosmosdbinitializer.interfaces;
using trifenix.agro.db.model.agro.core;

namespace trifenix.agro.authentication.operations {
    public class CosmosDbInitializer : ICosmosDbInitializer {

        private static CosmosStoreSettings StoreSettings;
        private readonly Assembly Assembly;
        private readonly bool AutoGuid;

        public CosmosDbInitializer(string CosmosDbName, string CosmosDbUri, string CosmosDbPrimaryKey, string AssemblyName, bool? autoGuid) {
            StoreSettings = new CosmosStoreSettings(CosmosDbName ?? "agrodb", CosmosDbUri ?? "https://agricola-jhm.documents.azure.com:443", CosmosDbPrimaryKey ?? "yG6EIAT1dKSBaS7oSZizTrWQGGfwSb2ot2prYJwQOLHYk3cGmzvvhGohSzFZYHueSFDiptUAqCQYYSeSetTiKw==");
            Assembly = !string.IsNullOrWhiteSpace(AssemblyName) ? Assembly.Load(AssemblyName) : typeof(BusinessName).Assembly;
            AutoGuid = autoGuid ?? false;
        }

        public static CosmosStore<T> CreateStoreInstance<T>() where T : class => new CosmosStore<T>(StoreSettings);

        public static T CreateEntityInstance<T>() => (T)Activator.CreateInstance(typeof(T));

        public static T GetValue<T>(JValue value) => typeof(T).Equals(typeof(DateTime)) ? (T)(object)DateTime.Parse(value.Value<string>()) : value.Value<T>();

        public void MapJsonToDB(dynamic json) {
            if (!(json is JObject))
                throw new Exception("\nError! Se espera un Json.");
            Type entityType;
            PropertyInfo prop;
            dynamic Store, dbInstance;
            List<string> Guids = new List<string>();
            if (AutoGuid) {
                foreach (var entity in json)
                    foreach (var jsonInstance in entity.Value)
                        Guids.Add(Guid.NewGuid().ToString("N"));
            }
            foreach (var entity in json) {
                entityType = Assembly.GetTypes().SingleOrDefault(type => type.Name.Equals(entity.Name));
                if (entityType == null)
                    continue;
                Store = typeof(CosmosDbInitializer).GetMethod("CreateStoreInstance").MakeGenericMethod(entityType).Invoke(null, null);
                dbInstance = typeof(CosmosDbInitializer).GetMethod("CreateEntityInstance").MakeGenericMethod(entityType).Invoke(null, null);
                foreach (var jsonInstance in entity.Value) {
                    var properties = jsonInstance.Properties();
                    foreach (var p in properties) {
                        prop = entityType.GetProperty(p.Name);
                        var value = typeof(CosmosDbInitializer).GetMethod("GetValue").MakeGenericMethod(prop.PropertyType).Invoke(null, new object[] { p.Value });
                        if (AutoGuid) {
                            int index = GetNumberOfGuid(value.ToString());
                            if(index != -1)
                                value = Guids.ElementAt(index);
                        }
                        prop?.SetValue(dbInstance, value);
                    }
                    if (!string.IsNullOrWhiteSpace(dbInstance.Id))
                        Store.UpsertAsync(dbInstance);
                }
            }
        }

        private int GetNumberOfGuid(string InputGuid) {
            string[] input = InputGuid.Split('(', ')');
            return input.Length == 3 && input[0].Equals("Guid") && int.TryParse(input[1], out int result) && result >=0 && input[2].Equals("")? result : -1;
        }

    }

}