using Cosmonaut;
using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.interfaces;
using trifenix.connect.agro.mdm_attributes;
using trifenix.connect.agro_model;
using trifenix.connect.util;

namespace trifenix.agro.db.applicationsReference
{

    public class MainGenericDb<T> : IMainGenericDb<T> where T : DocumentBase {

        public ICosmosStore<T> Store { get; }
        public ICosmosStore<EntityContainer> BatchStore { get; }

        public MainGenericDb(AgroDbArguments args) {
            var storeSettings = new CosmosStoreSettings(args.NameDb, args.EndPointUrl, args.PrimaryKey);
            Store = new CosmosStore<T>(storeSettings);
            BatchStore = new CosmosStore<EntityContainer>(storeSettings);
        }

        private EntityContainer GetContainer(T entity) => new EntityContainer { Id = Guid.NewGuid().ToString("N"), Entity = entity };

        private async Task<T> SetClientId(T entity, PropertyInfo prop_ClientId) {
            dynamic castedEntity = entity;
            var autoNumericSearchAttribute = Mdm.Reflection.Attributes.GetAttribute<AutoNumericSearchAttribute>(prop_ClientId);
            bool numerateByDependence = autoNumericSearchAttribute.Dependant.HasValue;
            if (numerateByDependence) {
                var prop_referenceToIndependent = entity.GetType().GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(ReferenceSearchAttribute)) && Mdm.Reflection.Attributes.GetAttribute<AutoNumericSearchAttribute>(prop).Index == autoNumericSearchAttribute.Dependant);
                var idIndependent = (string)prop_referenceToIndependent?.GetValue(entity);
                var query = $"SELECT * FROM c WHERE c.{prop_referenceToIndependent.Name} = '{idIndependent}'";
                var dependentElements = (IEnumerable<DocumentBase<int>>)await Store.QueryMultipleAsync(query);
                castedEntity.ClientId = dependentElements.Max(element => (int?)element.ClientId) ?? 0 + 1;
            } else
                castedEntity.ClientId = Store.Query().Max(element => (int?)((DocumentBase<int>)(object)element).ClientId) ?? 0 + 1;
            return castedEntity;
        }

        public async Task RenewClientIds() {
            var prop_ClientId = typeof(T).GetProperty("ClientId"); 
            if(prop_ClientId != null) {
                dynamic entities = await GetEntities().ToListAsync();
                var autoNumericSearchAttribute = Mdm.Reflection.Attributes.GetAttribute<AutoNumericSearchAttribute>(prop_ClientId);
                bool numerateByDependence = autoNumericSearchAttribute.Dependant.HasValue;
                if (numerateByDependence) {
                    var dictionary = new Dictionary<string,int>();
                    var prop_referenceToIndependent = typeof(T).GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(ReferenceSearchAttribute)) && Mdm.Reflection.Attributes.GetAttribute<AutoNumericSearchAttribute>(prop_ClientId).Index == autoNumericSearchAttribute.Dependant);
                    foreach (var entity in entities) {
                        var idIndependent = (string)prop_referenceToIndependent?.GetValue(entity);
                        if(dictionary.TryGetValue(idIndependent, out int max)) {
                            dictionary[idIndependent] = ++max;
                            entity.ClientId = max;
                        } else {
                            dictionary.Add(idIndependent,1);
                            entity.ClientId = 1;
                        }
                    }
                } else {
                    for (int index = 1; index <= entities.Count; index++)
                        entities[index-1].ClientId = index;
                }
                await Store.UpsertRangeAsync((IEnumerable<T>)entities);
            }
        }

        public async Task<string> CreateUpdate(T entity) {
            if (string.IsNullOrWhiteSpace(entity.Id))
                throw new NonIdException<DocumentBase>(entity);
            var alreadyExists = (await Store.QuerySingleAsync($"SELECT * FROM c WHERE c.Id = '{entity.Id}'")) != null;
            var prop_ClientId = entity.GetType().GetProperty("ClientId");
            if (!alreadyExists && prop_ClientId != null)
                entity = await SetClientId(entity, prop_ClientId);
            var result = await Store.UpsertAsync(entity);
            if (!result.IsSuccess)
                throw result.Exception;
            return result.Entity.Id;
        }

        public async Task<string> CreateEntityContainer(T entity) {
            if (string.IsNullOrWhiteSpace(entity.Id))
                throw new NonIdException<DocumentBase>(entity);
            var result = await BatchStore.UpsertAsync(GetContainer(entity));
            if (!result.IsSuccess)
                throw result.Exception;
            return result.Entity.Id;
        }

        public async Task<T> GetEntity(string id) => await Store.FindAsync(id);
       

        public async Task DeleteEntity(string id) {
            
            await Store.RemoveByIdAsync(id);
        } 

        public IQueryable<T> GetEntities() => Store.Query();

    }

}