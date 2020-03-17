using Cosmonaut.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using trifenix.agro.cosmosdbinitializer.interfaces;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.model.agro.core;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.authentication.operations {
    public class CosmosDbInitializer : ICosmosDbInitializer {

        //private static CosmosStoreSettings StoreSettings;
        private readonly Assembly Assembly_Inputs;
        private readonly Assembly Assembly_Entities;
        private readonly IAgroManager Manager;

        public CosmosDbInitializer(IAgroManager manager, string assembly_Inputs, string assembly_Entities) {
            Manager = manager;
            //StoreSettings = new CosmosStoreSettings(CosmosDbName ?? "agrodb", CosmosDbUri ?? "https://agricola-jhm.documents.azure.com:443", CosmosDbPrimaryKey ?? "yG6EIAT1dKSBaS7oSZizTrWQGGfwSb2ot2prYJwQOLHYk3cGmzvvhGohSzFZYHueSFDiptUAqCQYYSeSetTiKw==");
            Assembly_Inputs = !string.IsNullOrWhiteSpace(assembly_Inputs) ? Assembly.Load(assembly_Inputs) : typeof(BusinessNameInput).Assembly;
            Assembly_Entities = !string.IsNullOrWhiteSpace(assembly_Entities) ? Assembly.Load(assembly_Entities) : typeof(BusinessName).Assembly;
        }

        //public static CosmosStore<T> CreateStoreInstance<T>() where T : class => new CosmosStore<T>(StoreSettings);

        public static T CreateEntityInstance<T>() => (T)Activator.CreateInstance(typeof(T));

        public static T GetValue<T>(JValue value) => typeof(T).Equals(typeof(DateTime)) ? (T)(object)DateTime.Parse(value.Value<string>()) : value.Value<T>();

        public async Task<JsonResult> MapJsonToDB(dynamic json) {
            if (!(json is JObject))
                throw new Exception("\nError! Se espera un Json.");
            Type EntityType;
            PropertyInfo Prop;
            //dynamic Store;
            dynamic dbInstance, EntityOperations, Value;
            List<JProperty> JProperties;
            List<string> Guids = new List<string>();
            string idRelated;
            foreach (var entity in json) {
                EntityType = Assembly_Inputs.GetTypes().SingleOrDefault(type => type.Name.Equals($"{entity.Name}Input"));
                if (EntityType == null)
                    continue;
                EntityOperations = Manager.GetType().GetProperty(entity.Name).GetValue(Manager);
                //Store = typeof(CosmosDbInitializer).GetMethod("CreateStoreInstance").MakeGenericMethod(entityType).Invoke(null, null);
                dbInstance = typeof(CosmosDbInitializer).GetMethod("CreateEntityInstance").MakeGenericMethod(EntityType).Invoke(null, null);
                foreach (var jsonInstance in entity.Value) {
                    JProperties = ((IEnumerable<JProperty>)jsonInstance.Properties()).ToList();
                    foreach (var p in JProperties) {
                        Prop = EntityType.GetProperty(p.Name);
                        Value = typeof(CosmosDbInitializer).GetMethod("GetValue").MakeGenericMethod(Prop.PropertyType).Invoke(null, new object[] { p.Value });
                        int index = GetNumberOfGuid(Value.ToString());
                        if(index != -1 && index < Guids.Count)
                            Value = Guids.ElementAt(index);
                        Prop?.SetValue(dbInstance, Value);
                    }
                    try {
                        idRelated = (await EntityOperations.Save(dbInstance,true)).IdRelated;
                        Guids.Add(idRelated);
                    } catch(Exception ex) {
                        await Manager.BatchStore.RemoveAsync(container => true);
                        if (ex is Validation_Exception)
                            return new JsonResult(((Validation_Exception)ex).ErrorMessages);
                        var extPostError = new ExtPostErrorContainer<string> {
                            InternalException = ex,
                            Message = ex.Message,
                            MessageResult = ExtMessageResult.Error
                        };
                        return new JsonResult(extPostError.GetBase);
                    }
                }
            }
            var EntityContainers = await Manager.BatchStore.Query().ToListAsync();
            string EntityName;
            foreach (var container in EntityContainers) {
                JProperties = ((IEnumerable<JProperty>)container.Entity.Properties()).ToList();
                EntityName = (string)typeof(CosmosDbInitializer).GetMethod("GetValue").MakeGenericMethod(typeof(string)).Invoke(null, new object[] { JProperties.Find(property => property.Name.Equals("CosmosEntityName")).Value });
                EntityType = Assembly_Entities.GetTypes().SingleOrDefault(type => type.Name.Equals(EntityName));
                dbInstance = typeof(CosmosDbInitializer).GetMethod("CreateEntityInstance").MakeGenericMethod(EntityType).Invoke(null, null);
                JProperties.ForEach(Jprop => {
                    Prop = EntityType.GetProperty(Jprop.Name);
                    Value = typeof(CosmosDbInitializer).GetMethod("GetValue").MakeGenericMethod(Prop.PropertyType).Invoke(null, new object[] { Jprop.Value });
                    Prop?.SetValue(dbInstance, Value);
                });
                EntityOperations = Manager.GetType().GetProperty(EntityName).GetValue(Manager);
                await EntityOperations.Save(dbInstance);
            }
            await Manager.BatchStore.RemoveAsync(container => true);
            return new JsonResult("Operacion exitosa.");
        }



        private int GetNumberOfGuid(string InputGuid) {
            string[] input = InputGuid.Split('(', ')');
            return input.Length == 3 && input[0].Equals("Guid") && int.TryParse(input[1], out int result) && result >=0 && input[2].Equals("")? result : -1;
        }

    }

}