using Cosmonaut.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using trifenix.agro.cosmosdbinitializer.interfaces;
using trifenix.agro.db;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.model;
using trifenix.agro.db.model.core;
using trifenix.agro.enums;
using trifenix.agro.enums.input;
using trifenix.agro.external.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.authentication.operations {
    public class CosmosDbInitializer : ICosmosDbInitializer {

        private readonly Assembly Assembly_Inputs;
        private readonly Assembly Assembly_Entities;
        private readonly IAgroManager Manager;

        public CosmosDbInitializer(IAgroManager manager, string assembly_Inputs, string assembly_Entities) {
            Manager = manager;
            Assembly_Inputs = !string.IsNullOrWhiteSpace(assembly_Inputs) ? Assembly.Load(assembly_Inputs) : typeof(BusinessNameInput).Assembly;
            Assembly_Entities = !string.IsNullOrWhiteSpace(assembly_Entities) ? Assembly.Load(assembly_Entities) : typeof(BusinessName).Assembly;
        }

        public static T CreateEntityInstance<T>() => (T)Activator.CreateInstance(typeof(T));

        public static T GetValue<T>(JValue value) => typeof(T).Equals(typeof(DateTime)) ? (T)(object)DateTime.ParseExact(value.Value<string>(), "dd/MM/yyyy", CultureInfo.InvariantCulture) : value.Value<T>();

        private int GetGuid(string InputGuid) {
            string[] input = InputGuid.Split('(', ')');
            return input.Length == 3 && input[0].Equals("Guid") && int.TryParse(input[1], out int result) && result >=0 && input[2].Equals("")? result : -1;
        }

        private IGenericOperation<DocumentBase,InputBase> GetEntityOperations(string entityOperationName) => (IGenericOperation<DocumentBase,InputBase>)Manager.GetType().GetProperty(entityOperationName).GetValue(Manager);

        private object InvokeGenericMethod(string methodName, Type genericParameterType, object[] args) => typeof(CosmosDbInitializer).GetMethod(methodName).MakeGenericMethod(genericParameterType).Invoke(null, args);
        
        private Type GetTypeFromAssembly(Assembly assm, string typeName) => assm.GetTypes().SingleOrDefault(type => type.Name.Equals(typeName));
        private async Task SaveFromJsonToEntityContainers(JObject json) {
            Type EntityType;
            dynamic dbInstance, EntityOperations, Value;
            PropertyInfo Prop;
            List<JProperty> JProperties;
            List<string> Guids = new List<string>();
            string idRelated;
            foreach (dynamic entity in json) {
                EntityType = GetTypeFromAssembly(Assembly_Inputs, $"{entity.Name}Input");
                if (EntityType == null)
                    throw new MissingFieldException($"No se encontro la entidad con nombre {entity.Name}Input");
                EntityOperations = GetEntityOperations(entity.Name);
                dbInstance = InvokeGenericMethod("CreateEntityInstance", EntityType, null);
                foreach (var jsonInstance in entity.Value) {
                    JProperties = ((IEnumerable<JProperty>)jsonInstance.Properties()).ToList();
                    foreach (var jProp in JProperties) {
                        Prop = EntityType.GetProperty(jProp.Name);
                        Value = InvokeGenericMethod("GetValue", Prop.PropertyType, new object[] { jProp.Value });
                        if (Attribute.IsDefined(Prop, typeof(ReferenceAttribute))) {
                            int index = GetGuid(Value.ToString());
                            if (index != -1 && index < Guids.Count)
                                Value = Guids.ElementAt(index);
                        }
                        Prop?.SetValue(dbInstance, Value);
                    }
                    idRelated = (await EntityOperations.SaveInput(dbInstance,true)).IdRelated;
                    Guids.Add(idRelated);
                }
            }
        }

        private async Task SaveFromEntityContainersToDb(List<EntityContainer> containers) {
            string EntityName;
            Type EntityType;
            dynamic dbInstance, EntityOperations;
            object Value;
            PropertyInfo Prop;
            List<JProperty> JProperties;
            foreach (var container in containers) {
                JProperties = ((IEnumerable<JProperty>)container.Entity.Properties()).ToList();
                EntityName = (string)InvokeGenericMethod("GetValue", typeof(string), new object[] { JProperties.Find(property => property.Name.Equals("CosmosEntityName")).Value });
                EntityType = GetTypeFromAssembly(Assembly_Entities, EntityName);
                dbInstance = InvokeGenericMethod("CreateEntityInstance", EntityType, null);
                JProperties.ForEach(Jprop => {
                    Prop = EntityType.GetProperty(Jprop.Name);
                    Value = InvokeGenericMethod("GetValue", Prop.PropertyType, new object[] { Jprop.Value });
                    Prop?.SetValue(dbInstance, Value);
                });
                EntityOperations = GetEntityOperations(EntityName);
                await EntityOperations.Save(dbInstance);
            }
            await Manager.BatchStore.RemoveAsync(container => true);
        }

        public async Task<JsonResult> MapJsonToDB(JObject json) {
            Stopwatch timer = Stopwatch.StartNew();
            try {
                await SaveFromJsonToEntityContainers(json);
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
            var EntityContainers = await Manager.BatchStore.Query().ToListAsync();
            await SaveFromEntityContainersToDb(EntityContainers);
            timer.Stop();
            return new JsonResult($"Operacion exitosa. Tiempo transcurrido: {timer.Elapsed.ToString("hh\\:mm\\:ss")}");
        }

    }

}